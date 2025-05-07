using BookEvent.Core.Application.Abstraction.Services.Auth;
using BookEvent.Core.Domain.Entities._Identity;
using BookEvent.Shared.Errors.Models;
using BookEvent.Shared.Models.Auth;
using BookEvent.Shared.Models.Roles;
using BookEvent.Shared.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookEvent.Core.Application.Services.Auth
{
    public class AuthService(UserManager<ApplicationUser> userManager
        , SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager
        , IOptions<JwtSettings> jwtsettings) : IAuthService
    {
        private readonly JwtSettings _jwtsettings = jwtsettings.Value;


        public async Task<UserToRetuen> RegisterAsync(RegisterDto registerDto)
        {
            if (userManager.Users.Any(e => e.Email == registerDto.Email))
                throw new BadRequestExeption("Email Already Exists");
            if (userManager.Users.Any(e => e.PhoneNumber == registerDto.PhoneNumber))
                throw new BadRequestExeption("Phone Number Already Exists");
            var user = new ApplicationUser()
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.Email,

            };






            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                throw new ValidationExeption { Errors = result.Errors.Select(p => p.Description) };

            //await ConfirmationCodeSendByEmailAsync(new ForgetPasswordByEmailDto { Email = user.Email! });

            var roleResult = await userManager.AddToRoleAsync(user, Roles.User);
            if (!roleResult.Succeeded)
                throw new ValidationExeption { Errors = roleResult.Errors.Select(e => e.Description) };

            return CreateUserResponse(user);
        }

        public async Task<UserToRetuen> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                throw new UnAuthorizedExeption("Invalid Login");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

            if (result.IsNotAllowed)
                throw new UnAuthorizedExeption("Email is Not Confirmed");

            if (result.IsLockedOut)
                throw new UnAuthorizedExeption("Email is Locked Out");

            if (!result.Succeeded)
                throw new UnAuthorizedExeption("Invalid Login");

            var token = await GenerateTokenAsync(user);

            var response = new UserToRetuen()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!,
                Token = token

            };
            response.Token = token;
            await CheckRefreshToken(userManager, user, response);
            return response;






        }



        public async Task<UserToRetuen> GetRefreshToken(RefreshDto refreshDto, CancellationToken cancellationToken = default)
        {
            var userId = ValidateToken(refreshDto.Token);

            if (userId is null) throw new NotFoundExeption("User id Not Found", nameof(userId));

            var user = await userManager.FindByIdAsync(userId);
            if (user is null) throw new NotFoundExeption("User Do Not Exists", nameof(user.Id));

            var UserRefreshToken = user!.RefreshTokens.SingleOrDefault(x => x.Token == refreshDto.RefreshToken && x.IsActice);

            if (UserRefreshToken is null) throw new NotFoundExeption("Invalid Token", nameof(userId));

            UserRefreshToken.RevokedOn = DateTime.UtcNow;

            var newtoken = await GenerateTokenAsync(user);

            var newrefreshtoken = GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken()
            {
                Token = newrefreshtoken.Token,
                ExpireOn = newrefreshtoken.ExpireOn
            });

            await userManager.UpdateAsync(user);

            return new UserToRetuen()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!,

                Token = newtoken,
                RefreshToken = newrefreshtoken.Token,
                RefreshTokenExpirationDate = newrefreshtoken.ExpireOn,


            };
        }

        public async Task<bool> RevokeRefreshTokenAsync(RefreshDto refreshDto, CancellationToken cancellationToken = default)
        {
            var userId = ValidateToken(refreshDto.Token);

            if (userId is null) return false;

            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return false;

            var UserRefreshToken = user!.RefreshTokens.SingleOrDefault(x => x.Token == refreshDto.RefreshToken && x.IsActice);

            if (UserRefreshToken is null) return false;

            UserRefreshToken.RevokedOn = DateTime.UtcNow;

            await userManager.UpdateAsync(user);
            return true;
        }









        #region Custom Functions
        private async Task CheckRefreshToken(UserManager<ApplicationUser> userManager, ApplicationUser? user, UserToRetuen response)
        {
            if (user!.RefreshTokens.Any(t => t.IsActice))
            {
                var acticetoken = user.RefreshTokens.FirstOrDefault(x => x.IsActice);
                response.RefreshToken = acticetoken!.Token;
                response.RefreshTokenExpirationDate = acticetoken.ExpireOn;
            }
            else
            {

                var refreshtoken = GenerateRefreshToken();
                response.RefreshToken = refreshtoken.Token;
                response.RefreshTokenExpirationDate = refreshtoken.ExpireOn;

                user.RefreshTokens.Add(new RefreshToken()
                {
                    Token = refreshtoken.Token,
                    ExpireOn = refreshtoken.ExpireOn,
                });
                await userManager.UpdateAsync(user);
            }
        }
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            var genrator = new RNGCryptoServiceProvider();

            genrator.GetBytes(randomNumber);

            return new RefreshToken()
            {
                Token = Convert.ToBase64String(randomNumber),
                CreatedOn = DateTime.UtcNow,
                ExpireOn = DateTime.UtcNow.AddDays(_jwtsettings.JWTRefreshTokenExpire)


            };


        }
        private string? ValidateToken(string token)
        {
            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtsettings.Key));

            var tokenhandler = new JwtSecurityTokenHandler();

            try
            {
                tokenhandler.ValidateToken(token, new TokenValidationParameters()
                {
                    IssuerSigningKey = authkey,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero,
                }, out SecurityToken securityToken);

                var securitytokenobj = (JwtSecurityToken)securityToken;

                return securitytokenobj.Claims.First(x => x.Type == ClaimTypes.PrimarySid).Value;
            }
            catch (Exception)
            {

                return null;
            }
        }
        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var userclaims = await userManager.GetClaimsAsync(user);

            var userrolesclaims = new List<Claim>();

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                userrolesclaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            IEnumerable<Claim> claims;

            claims = new List<Claim>()
                {
                new Claim(ClaimTypes.PrimarySid,user.Id),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber!),
                new Claim(ClaimTypes.Name,user.FullName)

            }
           .Union(userclaims)
           .Union(userrolesclaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtsettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var tokenObj = new JwtSecurityToken(

                issuer: _jwtsettings.Issuer,
                audience: _jwtsettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtsettings.DurationInMinitutes),
                claims: claims,
                signingCredentials: signingCredentials
                );


            return new JwtSecurityTokenHandler().WriteToken(tokenObj);
        }


        private UserToRetuen CreateUserResponse(ApplicationUser user)
        {



            var response = new UserToRetuen
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!,
                Token = GenerateTokenAsync(user).Result,
            };


            return response;

        }

        public async Task<UserToRetuen> GetCurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid)?.Value;
            if (userId is null) throw new NotFoundExeption("User id Not Found", nameof(userId));
            var user = await userManager.FindByIdAsync(userId);
            if (user is null) throw new NotFoundExeption("User Do Not Exists", nameof(user.Id));
            var response = new UserToRetuen()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!,
                Token = GenerateTokenAsync(user).Result
            };
            return response;
        }

        #endregion

    }
}
