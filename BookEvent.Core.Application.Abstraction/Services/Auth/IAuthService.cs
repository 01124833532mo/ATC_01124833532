using BookEvent.Shared.Models.Auth;
using System.Security.Claims;

namespace BookEvent.Core.Application.Abstraction.Services.Auth
{
    public interface IAuthService
    {
        Task<UserToRetuen> GetRefreshToken(RefreshDto refreshDto, CancellationToken cancellationToken = default);

        Task<bool> RevokeRefreshTokenAsync(RefreshDto refreshDto, CancellationToken cancellationToken = default);
        Task<UserToRetuen> LoginAsync(LoginDto loginDto);

        Task<UserToRetuen> RegisterAsync(RegisterDto registerDto);
        Task<UserToRetuen> GetCurrentUser(ClaimsPrincipal claimsPrincipal);

        Task<ChangePasswordToReturn> ChangePasswordAsync(ClaimsPrincipal claims, ChangePasswordDto changePasswordDto);



    }
}
