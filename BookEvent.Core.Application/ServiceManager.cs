using BookEvent.Core.Application.Abstraction;
using BookEvent.Core.Application.Abstraction.Services.Auth;
using BookEvent.Core.Application.Abstraction.Services.Categories;

namespace BookEvent.Core.Application
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<ICategoriesService> _categoriesService;

        public ServiceManager(Func<IAuthService> authfactory, Func<ICategoriesService> categoryfactory)

        {
            _authService = new Lazy<IAuthService>(authfactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _categoriesService = new Lazy<ICategoriesService>(categoryfactory, LazyThreadSafetyMode.ExecutionAndPublication);


        }



        public IAuthService AuthService => _authService.Value;

        public ICategoriesService CategoriesService => _categoriesService.Value;
    }
}
