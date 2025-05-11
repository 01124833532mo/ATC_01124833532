using BookEvent.Core.Application.Abstraction.Services.Auth;
using BookEvent.Core.Application.Abstraction.Services.Booking;
using BookEvent.Core.Application.Abstraction.Services.Categories;
using BookEvent.Core.Application.Abstraction.Services.Events;

namespace BookEvent.Core.Application.Abstraction
{
    public interface IServiceManager
    {
        public IAuthService AuthService { get; }
        public ICategoriesService CategoriesService { get; }
        public IEventService EventService { get; }
        public IBookService BookService { get; }

    }
}
