using BookEvent.Core.Application.Abstraction;
using BookEvent.Core.Application.Abstraction.Services.Auth;
using BookEvent.Core.Application.Abstraction.Services.Categories;
using BookEvent.Core.Application.Abstraction.Services.Events;

namespace BookEvent.Core.Application
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<ICategoriesService> _categoriesService;
        private readonly Lazy<IEventService>  _eventService;

        public ServiceManager(Func<IAuthService> authfactory, Func<ICategoriesService> categoryfactory,Func<IEventService> eventfactory)

        {
            _authService = new Lazy<IAuthService>(authfactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _categoriesService = new Lazy<ICategoriesService>(categoryfactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _eventService = new Lazy<IEventService>(eventfactory, LazyThreadSafetyMode.ExecutionAndPublication);


        }



        public IAuthService AuthService => _authService.Value;

        public ICategoriesService CategoriesService => _categoriesService.Value;

        public IEventService EventService => _eventService.Value;
    }
}
