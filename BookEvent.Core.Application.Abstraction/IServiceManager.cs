using BookEvent.Core.Application.Abstraction.Services.Auth;

namespace BookEvent.Core.Application.Abstraction
{
    public interface IServiceManager
    {
        public IAuthService AuthService { get; }

    }
}
