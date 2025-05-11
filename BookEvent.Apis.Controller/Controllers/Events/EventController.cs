using BookEvent.Apis.Controller.Controllers.Base;
using BookEvent.Core.Application.Abstraction;
using BookEvent.Core.Application.Abstraction.Common;
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
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("UpdateEvent/{id}")]
        public async Task<ActionResult> UpdateEvent([FromRoute] int id, [FromForm] EventDto eventdto, CancellationToken cancellationToken)
        {
            var result = await serviceManager.EventService.UpdateEventAsync(id, eventdto, cancellationToken);
            return NewResult(result);
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("DeleteEvent/{id}")]
        public async Task<ActionResult> DeleteEvent([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await serviceManager.EventService.DeleteAsync(id, cancellationToken);
            return NewResult(result);
        }
        [HttpGet("GetEvent/{id}")]
        public async Task<ActionResult> GetEvent([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await serviceManager.EventService.GetEventAsync(id, cancellationToken);
            return NewResult(result);
        }
        [HttpGet("GetAllEvents")]
        public async Task<ActionResult<Pagination<EventResponse>>> GetAllEvents([FromQuery] SpecParams specParams, CancellationToken cancellationToken)
        {
            var products = await serviceManager.EventService.GetAllEventsAsynce(specParams, cancellationToken);
            return Ok(products);
        }


    }
}
