using AutoMapper;
using BookEvent.Core.Application.Abstraction;
using BookEvent.Core.Application.Abstraction.Bases;
using BookEvent.Core.Application.Abstraction.Services.Booking;
using BookEvent.Core.Domain.Contracts.Persestence;
using BookEvent.Core.Domain.Entities.Books;
using BookEvent.Core.Domain.Entities.Events;
using BookEvent.Shared.Models.Booking;
using Microsoft.EntityFrameworkCore;

namespace BookEvent.Core.Application.Services.Booking
{
    public class BookService(IUnitOfWork unitOfWork, IMapper mapper, ILoggedInUserService userService) : ResponseHandler, IBookService
    {
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
    }
}
