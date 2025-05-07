using BookEvent.Apis.Controller.Controllers.Base;
using BookEvent.Core.Application.Abstraction;
using BookEvent.Shared.Models.Auth;
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
    }
}
