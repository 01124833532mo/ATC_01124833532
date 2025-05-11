using BookEvent.Core.Application.Abstraction.Bases;
using BookEvent.Shared.Models.Booking;

namespace BookEvent.Core.Application.Abstraction.Services.Booking
{
    public interface IBookService
    {

        Task<Response<BookToReturn>> CreateBookAsync(CreateBookDto createBookDto, CancellationToken cancellationToken);


    }
}
