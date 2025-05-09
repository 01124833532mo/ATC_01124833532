using BookEvent.Core.Application.Abstraction.Services.Auth;
using BookEvent.Core.Application.Abstraction.Services.Categories;

namespace BookEvent.Core.Application.Abstraction
{
    public interface IServiceManager
    {
        public IAuthService AuthService { get; }
        public ICategoriesService CategoriesService { get; }

    }
}
