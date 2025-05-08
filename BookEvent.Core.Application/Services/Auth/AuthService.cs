using BookEvent.Core.Application.Abstraction.Services.Auth;
using BookEvent.Core.Application.Abstraction.Services.Emails;
using BookEvent.Core.Domain.Entities._Identity;
using BookEvent.Shared.Errors.Models;
using BookEvent.Shared.Models._Common.Emails;
using BookEvent.Shared.Models.Auth;
using BookEvent.Shared.Models.Roles;
using BookEvent.Shared.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        IEmailService emailService
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

            var email = new SendCodeByEmailDto() { Email = user.Email! };

            await SendCodeByEmail(email, true);

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
        public async Task<SuccessDto> SendCodeByEmail(SendCodeByEmailDto emailDto, bool IsRegistration = false)
        {
            var user = await userManager.Users.Where(u => u.Email == emailDto.Email).FirstOrDefaultAsync();

            if (user is null)
                throw new BadRequestExeption("Invalid Email");

            var code = RandomNumberGenerator.GetInt32(100_0, 999_9);
            var codeExpire = DateTime.UtcNow.AddMinutes(15);

            if (IsRegistration)
            {
                user.ResetCode = code;
                user.ResetCodeExpiry = codeExpire;
            }
            else
            {
                user.ResetCode = code;
                user.ResetCodeExpiry = codeExpire;
            }

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new BadRequestExeption("Something Went Wrong While Sending Code");

            var email = new Email()
            {
                To = emailDto.Email,
                Subject = IsRegistration ? "Confirm Your CarCare Account" : "Your CarCare Password Reset Code",
                Body = IsRegistration ? BuildConfirmationEmail(code) : BuildResetPasswordEmail(code),
                IsBodyHtml = true
            };

            await emailService.SendEmail(email);

            return new SuccessDto()
            {
                Status = "Success",
                Message = IsRegistration ? "Confirmation code has been sent" : "Reset code has been sent"
            };
        }

        public async Task<SuccessDto> ConfirmEmailAsync(ConfirmationEmailCodeDto codeDto)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(e => e.Email == codeDto.Email);

            if (user is null)
                throw new BadRequestExeption("Invalid Email");

            if (user.ResetCode != codeDto.ConfirmationCode)
                throw new BadRequestExeption("The Provided Code Is Invalid");

            if (user.ResetCodeExpiry < DateTime.UtcNow)
                throw new BadRequestExeption("The Provided Code Has Been Expired");

            user.EmailConfirmed = true;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new BadRequestExeption("Something Went Wrong While Confirming Email");

            var SuccessObj = new SuccessDto()
            {
                Status = "Success",
                Message = "Email Has Been Confirmed"
            };

            return SuccessObj;
        }

        public async Task<SuccessDto> VerifyCodeByEmailAsync(ResetCodeConfirmationByEmailDto resetCodeDto)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(e => e.Email == resetCodeDto.Email);

            if (user is null)
                throw new BadRequestExeption("Invalid Email");

            if (user.ResetCode != resetCodeDto.ResetCode)
                throw new BadRequestExeption("The Provided Code Is Invalid");

            if (user.ResetCodeExpiry < DateTime.UtcNow)
                throw new BadRequestExeption("The Provided Code Has Been Expired");

            var SuccessObj = new SuccessDto()
            {
                Status = "Success",
                Message = "Reset Code Is Verified, Please Proceed To Change Your Password"
            };

            return SuccessObj;
        }

        public async Task<UserToRetuen> ResetPasswordByEmailAsync(ResetPasswordByEmailDto resetCodeDto)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(e => e.Email == resetCodeDto.Email);

            if (user is null)
                throw new BadRequestExeption("Invalid Email");

            var RemovePass = await userManager.RemovePasswordAsync(user);

            if (!RemovePass.Succeeded)
                throw new BadRequestExeption("Something Went Wrong While Reseting Your Password");

            var newPass = await userManager.AddPasswordAsync(user, resetCodeDto.NewPassword);

            if (!newPass.Succeeded)
                throw new BadRequestExeption("Something Went Wrong While Reseting Your Password");

            var mappedUser = new UserToRetuen
            {
                FullName = user.FullName!,
                Id = user.Id,
                PhoneNumber = user.PhoneNumber!,
                Email = user.Email!,
                Token = await GenerateTokenAsync(user),
            };

            return mappedUser;
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


        private string BuildResetPasswordEmail(int resetCode)
        {
            return $@"
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
            .header {{ color: #2c3e50; border-bottom: 1px solid #eee; padding-bottom: 10px; }}
            .code {{ font-size: 24px; font-weight: bold; color: #e74c3c; margin: 20px 0; text-align: center; }}
            .footer {{ margin-top: 20px; font-size: 12px; color: #7f8c8d; border-top: 1px solid #eee; padding-top: 10px; }}
            .button {{ background-color: #3498db; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block; }}
        </style>
    </head>
    <body>
        <div class='header'>
            <h2>Password Reset Request</h2>
        </div>
        
        <p>We received a request to reset your password. Please use the following verification code:</p>
        
        <div class='code'>{resetCode}</div>
        
        <p>This code will expire in <strong>15 minutes</strong>. If you didn't request this, please ignore this email or contact support if you have questions.</p>
        
        <div class='footer'>
            <p>Thank you,<br>The Support Team</p>
        </div>
    </body>
    </html>";
        }
        private string BuildConfirmationEmail(int confirmationCode)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ color: #2c3e50; border-bottom: 1px solid #eee; padding-bottom: 10px; }}
        .code {{ font-size: 24px; font-weight: bold; color: #2ecc71; margin: 20px 0; text-align: center; }}
        .footer {{ margin-top: 20px; font-size: 12px; color: #7f8c8d; border-top: 1px solid #eee; padding-top: 10px; }}
        .button {{ background-color: #3498db; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block; }}
    </style>
</head>
<body>
    <div class='header'>
        <h2>Email Confirmation</h2>
    </div>
    
    <p>Thank you for registering with us. Please use the following verification code to confirm your email:</p>
    
    <div class='code'>{confirmationCode}</div>
    
    <p>This code will expire in <strong>15 minutes</strong>. If you didn't register, please ignore this email.</p>
    
    <div class='footer'>
        <p>Thank you,<br>The Support Team</p>
    </div>
</body>
</html>";
        }


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
                expires: DateTime.UtcNow.AddDays(_jwtsettings.DurationInMinitutes),
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

        public async Task<ChangePasswordToReturn> ChangePasswordAsync(ClaimsPrincipal claims, ChangePasswordDto changePasswordDto)
        {
            var userId = claims.FindFirst(ClaimTypes.PrimarySid)?.Value;

            if (userId is null) throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");


            // Retrieve the user from the database
            var user = await userManager.FindByIdAsync(userId);

            if (user is null) throw new NotFoundExeption("No User For This Id", nameof(userId));


            // Verify the current password
            var isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword);

            if (!isCurrentPasswordValid)
            {
                throw new BadRequestExeption("This Current Password InCorrect");
            }

            // Change the password
            var result = await userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                throw new ValidationExeption() { Errors = result.Errors.Select(p => p.Description) };
            }

            // Optionally, generate a new token for the user
            var newToken = await GenerateTokenAsync(user);

            return new ChangePasswordToReturn()
            {
                Message = "Password changed successfully.",
                Token = newToken
            };
        }




        #endregion

        public async Task<UserToRetuen> UpdateAppUserBySelf(ClaimsPrincipal claims, UpdateUserDto appUserDto)
        {
            var userId = claims.FindFirst(ClaimTypes.PrimarySid)?.Value;
            if (userId is null) throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");
            var user = await userManager.FindByIdAsync(userId);
            if (user is null) throw new NotFoundExeption("No User For This Id", nameof(userId));
            user.FullName = appUserDto.FullName;
            user.PhoneNumber = appUserDto.PhoneNumber;
            user.Email = appUserDto.Email;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new ValidationExeption { Errors = result.Errors.Select(p => p.Description) };
            return CreateUserResponse(user);
        }

    }
}
