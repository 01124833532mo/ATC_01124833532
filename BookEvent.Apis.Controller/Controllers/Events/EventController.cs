using BookEvent.Apis.Controller.Controllers.Base;
using BookEvent.Core.Application.Abstraction;
using BookEvent.Shared.Models.Events;
using BookEvent.Shared.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookEvent.Apis.Controller.Controllers.Events
{
    [Authorize]

    public class EventController(IServiceManager serviceManager) : BaseApiController
    {
        [Authorize(Roles = Roles.Admin)]
        [HttpPost("CreateEvent")]
        public async Task<ActionResult> CreateEvent([FromForm] EventDto eventdto, CancellationToken cancellationToken)
        {
            var result = await serviceManager.EventService.CreateEventAsync(eventdto, cancellationToken);
            return NewResult(result);
        }


    }
}
