﻿using BookEvent.Core.Application.Abstraction.Bases;
using BookEvent.Core.Application.Abstraction.Common;
using BookEvent.Shared.Models.Events;

namespace BookEvent.Core.Application.Abstraction.Services.Events
{
    public interface IEventService
    {
        Task<Response<EventResponse>> CreateEventAsync(EventDto eventRequest, CancellationToken cancellationToken = default);
        Task<Response<EventResponse>> UpdateEventAsync(int id, EventDto eventRequest, CancellationToken cancellationToken = default);
        Task<Response<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<Response<EventResponse>> GetEventAsync(int id, CancellationToken cancellationToken = default);
        Task<Pagination<EventResponse>> GetAllEventsAsynce(SpecParams specParams, CancellationToken cancellationToken = default);
    }
}
