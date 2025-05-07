using BookEvent.Core.Application.Abstraction;
using BookEvent.Core.Application.Abstraction.Services.Auth;

namespace BookEvent.Core.Application
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthService> _authService;

        public ServiceManager(Func<IAuthService> authfactory)

        {
            _authService = new Lazy<IAuthService>(authfactory, LazyThreadSafetyMode.ExecutionAndPublication);


        }



        public IAuthService AuthService => _authService.Value;

    }
}
