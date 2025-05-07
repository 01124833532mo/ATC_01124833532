using BookEvent.Shared.Models.Auth;

namespace BookEvent.Core.Application.Abstraction.Services.Auth
{
    public interface IAuthService
    {
        Task<UserToRetuen> GetRefreshToken(RefreshDto refreshDto, CancellationToken cancellationToken = default);

        Task<bool> RevokeRefreshTokenAsync(RefreshDto refreshDto, CancellationToken cancellationToken = default);

    }
}
