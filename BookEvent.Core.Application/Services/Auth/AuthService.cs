using BookEvent.Core.Application.Abstraction.Services.Auth;
using BookEvent.Core.Domain.Entities._Identity;
using BookEvent.Shared.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookEvent.Core.Application.Services.Auth
{
    public class AuthService(UserManager<ApplicationUser> userManager
        , SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager
        , IOptions<JwtSettings> jwtsettings) : IAuthService
    {
        private readonly JwtSettings _jwtsettings = jwtsettings.Value;









        #region Custom Functions

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
        #endregion

    }
}
