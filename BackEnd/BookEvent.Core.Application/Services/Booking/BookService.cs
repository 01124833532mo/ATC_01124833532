using AutoMapper;
using BookEvent.Core.Application.Abstraction;
using BookEvent.Core.Application.Abstraction.Bases;
using BookEvent.Core.Application.Abstraction.Services.Booking;
using BookEvent.Core.Domain.Contracts.Persestence;
using BookEvent.Core.Domain.Entities.Books;
using BookEvent.Core.Domain.Entities.Events;
using BookEvent.Shared.Models.Booking;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookEvent.Core.Application.Services.Booking
{
    public class BookService(IUnitOfWork unitOfWork, IMapper mapper, ILoggedInUserService userService) : ResponseHandler, IBookService
    {
        public async Task<Response<string>> CancelBookAsync(ClaimsPrincipal principal, int id, CancellationToken cancellationToken = default)
        {
            var bookrepo = unitOfWork.GetRepository<Book, int>();
            var checkexsistingbook = await bookrepo.GetAsync(id, cancellationToken);

            if (checkexsistingbook is null)
                return NotFound<string>("Booking Not Found");

            var userid = principal.FindFirstValue(ClaimTypes.PrimarySid);
            if (checkexsistingbook!.UserId != userid)
                return Unauthorized<string>("You Are Not Authorized to Cancel This Booking");

            bookrepo.Delete(checkexsistingbook);
            var complete = await unitOfWork.CompleteAsync() > 0;
            if (!complete) return BadRequest<string>("Error Occured While Cancelling Booking");
            return Deleted<string>("Booking Cancelled Successfully");


        }

        public async Task<Response<BookToReturn>> CreateBookAsync(CreateBookDto createBookDto, CancellationToken cancellationToken)
        {
            var existingEvent = await unitOfWork.GetRepository<Event, int>().GetAsync(createBookDto.EventId, cancellationToken);

            if (existingEvent is null)
            {
                return BadRequest<BookToReturn>("Event Not Found");
            }

            var bookRepo = unitOfWork.GetRepository<Book, int>();
            var book = mapper.Map<Book>(createBookDto);
            book.UserId = userService.UserId!;
            // Check if already booked
            var existingBooking = await bookRepo
                             .GetQuarable()
                            .Where(b => b.EventId == book.EventId && b.UserId == book.UserId)
                               .FirstOrDefaultAsync(cancellationToken);
            if (existingBooking != null)
            {
                return BadRequest<BookToReturn>("You have already booked this event.");
            }

            try
            {
                await bookRepo.AddAsync(book);

            }
            catch (Exception ex)
            {

                return BadRequest<BookToReturn>(ex.Message);
            }
            var complete = await unitOfWork.CompleteAsync() > 0;
            if (!complete) return BadRequest<BookToReturn>("Error Occured While Creating Booking");
            var bookToReturn = mapper.Map<BookToReturn>(book);
            bookToReturn.UserName = userService.FullName!;
            return Created(bookToReturn);

        }

        public async Task<Response<IReadOnlyList<BookToReturn>>> GetAllBooksForUserAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        {
            var bookrepo = unitOfWork.GetRepository<Book, int>();
            var userid = principal.FindFirstValue(ClaimTypes.PrimarySid);
            var books = await bookrepo
                .GetQuarable()
                .Include(b => b.Event)
                .Include(b => b.User)
                .Where(b => b.UserId == userid)
                .ToListAsync(cancellationToken);
            if (books is null || books.Count == 0)
                return NotFound<IReadOnlyList<BookToReturn>>("No Bookings Found");
            var bookToReturn = mapper.Map<IReadOnlyList<BookToReturn>>(books);
            var count = books.Count;

            return Success(bookToReturn, count);
        }
    }
}
