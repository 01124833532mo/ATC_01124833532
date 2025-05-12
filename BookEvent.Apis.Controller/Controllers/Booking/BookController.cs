using BookEvent.Apis.Controller.Controllers.Base;
using BookEvent.Core.Application.Abstraction;
using BookEvent.Shared.Models.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookEvent.Apis.Controller.Controllers.Booking
{
    [Authorize]
    public class BookController(IServiceManager serviceManager) : BaseApiController
    {


        [HttpPost("CreateBook")]
        public async Task<ActionResult> CreateBook([FromBody] CreateBookDto categorydto, CancellationToken cancellationToken)
        {
            var result = await serviceManager.BookService.CreateBookAsync(categorydto, cancellationToken);
            return NewResult(result);

        }
        [HttpDelete("CancelBook/{id}")]
        public async Task<ActionResult> CancelBook(int id, CancellationToken cancellationToken)
        {
            var result = await serviceManager.BookService.CancelBookAsync(User, id, cancellationToken);
            return NewResult(result);
        }
    }
}
