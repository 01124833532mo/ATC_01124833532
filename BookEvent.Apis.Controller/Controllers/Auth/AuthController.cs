using BookEvent.Apis.Controller.Controllers.Base;
using BookEvent.Core.Application.Abstraction;
using BookEvent.Shared.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookEvent.Apis.Controller.Controllers.Auth
{
    public class AuthController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpPost("Get-Refresh-Token")]

        public async Task<ActionResult<UserToRetuen>> RefreshToken([FromBody] RefreshDto model)
        {
            var result = await serviceManager.AuthService.GetRefreshToken(model);
            return Ok(result);
        }

        [HttpPost("Revoke-Refresh-Token")]
        public async Task<ActionResult> RevokeRefreshToken([FromBody] RefreshDto model)
        {
            var result = await serviceManager.AuthService.RevokeRefreshTokenAsync(model);
            return result is false ? BadRequest("Operation Not Successed") : Ok(result);

        }
        [HttpPost("login")]

        public async Task<ActionResult<UserToRetuen>> Login(LoginDto model)
        {
            var result = await serviceManager.AuthService.LoginAsync(model);
            return Ok(result);
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserToRetuen>> Register(RegisterDto model)
        {
            var result = await serviceManager.AuthService.RegisterAsync(model);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserToRetuen>> GetCurrentUser()
        {
            var result = await serviceManager.AuthService.GetCurrentUser(User);
            return Ok(result);
        }
        [Authorize]

        [HttpPost("Change-Password")]
        public async Task<ActionResult<ChangePasswordDto>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var result = await serviceManager.AuthService.ChangePasswordAsync(User, changePasswordDto);
            return Ok(result);
        }
        [HttpPost("Send-Code-By-Email")]
        public async Task<ActionResult<SuccessDto>> SendCodeByEmail([FromBody] SendCodeByEmailDto emailDto)
        {
            var result = await serviceManager.AuthService.SendCodeByEmail(emailDto);
            return Ok(result);
        }
        [HttpPost("Verify-Code-By-Email")]
        public async Task<ActionResult<SuccessDto>> VerifyCodeByEmail([FromBody] ResetCodeConfirmationByEmailDto resetCodeDto)
        {
            var result = await serviceManager.AuthService.VerifyCodeByEmailAsync(resetCodeDto);
            return Ok(result);
        }
        [HttpPut("Reset-Password-By-Email")]
        public async Task<ActionResult<UserToRetuen>> ResetPasswordByEmail([FromBody] ResetPasswordByEmailDto resetCodeDto)
        {
            var result = await serviceManager.AuthService.ResetPasswordByEmailAsync(resetCodeDto);
            return Ok(result);
        }
        [HttpPost("Confirm-Email")]
        public async Task<ActionResult<SuccessDto>> ConfirmEmail([FromBody] ConfirmationEmailCodeDto codeDto)
        {
            var result = await serviceManager.AuthService.ConfirmEmailAsync(codeDto);
            return Ok(result);
        }
        [Authorize]
        [HttpPut("Update-User-By-Self")]
        public async Task<ActionResult<UserToRetuen>> UpdateUserBySelf([FromBody] UpdateUserDto appUserDto)
        {
            var result = await serviceManager.AuthService.UpdateAppUserBySelf(User, appUserDto);
            return Ok(result);
        }
    }
}
