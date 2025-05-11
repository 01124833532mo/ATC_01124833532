using BookEvent.Core.Application.Abstraction.Bases;
using BookEvent.Shared.Models.Events;

namespace BookEvent.Core.Application.Abstraction.Services.Events
{
    public interface IEventService
    {
        Task<Response<EventResponse>> CreateEventAsync(EventDto eventRequest, CancellationToken cancellationToken);
    }
}
