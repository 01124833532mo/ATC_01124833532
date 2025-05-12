using BookEvent.Core.Application.Abstraction.Bases;
using BookEvent.Shared.Models.Booking;
using System.Security.Claims;

namespace BookEvent.Core.Application.Abstraction.Services.Booking
{
    public interface IBookService
    {

        Task<Response<BookToReturn>> CreateBookAsync(CreateBookDto createBookDto, CancellationToken cancellationToken);

        Task<Response<string>> CancelBookAsync(ClaimsPrincipal principal, int id, CancellationToken cancellationToken = default);


    }
}
