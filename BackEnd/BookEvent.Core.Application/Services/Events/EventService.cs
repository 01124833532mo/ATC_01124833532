using AutoMapper;
using BookEvent.Core.Application.Abstraction.Bases;
using BookEvent.Core.Application.Abstraction.Common;
using BookEvent.Core.Application.Abstraction.Common.Contracts.Infrastracture;
using BookEvent.Core.Application.Abstraction.Services.Events;
using BookEvent.Core.Domain.Contracts.Persestence;
using BookEvent.Core.Domain.Entities.Categories;
using BookEvent.Core.Domain.Entities.Events;
using BookEvent.Core.Domain.Specifications.Events;
using BookEvent.Shared.Models.Events;

namespace BookEvent.Core.Application.Services.Events
{
    public class EventService(IUnitOfWork unitOfWork, IMapper mapper, IAttachmentService attachmentService) : ResponseHandler, IEventService
    {
        public async Task<Response<EventResponse>> CreateEventAsync(EventDto eventRequest, CancellationToken cancellationToken)
        {

            var category = await unitOfWork.GetRepository<Category, int>().GetAsync(eventRequest.CategoryId ?? 0, cancellationToken);
            if (category is null) return NotFound<EventResponse>("Category Not Found");

            var eventRepo = unitOfWork.GetRepository<Event, int>();

            var eventEntity = mapper.Map<Event>(eventRequest);
            if (eventRequest.ImagePath is not null)
            {
                var imagePath = await attachmentService.UploadAsynce(eventRequest.ImagePath, "Events");
                if (imagePath is null)
                {
                    return BadRequest<EventResponse>("Error Occure While Updating Category");
                }
                eventEntity.ImagePath = imagePath;
            }
            try
            {
                await eventRepo.AddAsync(eventEntity);

            }
            catch (Exception ex)
            {
                return BadRequest<EventResponse>(ex.Message);
            }

            var complete = await unitOfWork.CompleteAsync() > 0;
            if (!complete) return BadRequest<EventResponse>("Error Occure While Creating Event");

            var eventResponse = mapper.Map<EventResponse>(eventEntity);
            return Created(eventResponse);

        }


        public async Task<Response<EventResponse>> UpdateEventAsync(int id, EventDto eventRequest, CancellationToken cancellationToken)
        {
            var eventRepo = unitOfWork.GetRepository<Event, int>();
            var eventEntity = await eventRepo.GetAsync(id, cancellationToken);
            if (eventEntity is null) return NotFound<EventResponse>("Event Not Found");
            var category = await unitOfWork.GetRepository<Category, int>().GetAsync(eventRequest.CategoryId ?? 0, cancellationToken);
            if (category is null) return NotFound<EventResponse>("Category Not Found");

            mapper.Map(eventRequest, eventEntity);

            if (eventRequest.ImagePath is not null)
            {
                var imagePath = await attachmentService.UploadAsynce(eventRequest.ImagePath, "Events");
                if (imagePath is null)
                {
                    return BadRequest<EventResponse>("Error Occure While Updating Category");
                }
                eventEntity.ImagePath = imagePath;
            }
            try
            {
                eventRepo.Update(eventEntity);
            }
            catch (Exception ex)
            {
                return BadRequest<EventResponse>(ex.Message);
            }
            var complete = await unitOfWork.CompleteAsync() > 0;
            if (!complete) return BadRequest<EventResponse>("Error Occure While Updating Event");
            var eventResponse = mapper.Map<EventResponse>(eventEntity);
            return Success(eventResponse);
        }


        public async Task<Response<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var eventRepo = unitOfWork.GetRepository<Event, int>();
            var eventEntity = await eventRepo.GetAsync(id, cancellationToken);
            if (eventEntity is null) return NotFound<string>("Event Not Found");
            try
            {
                eventRepo.Delete(eventEntity);
            }
            catch (Exception ex)
            {
                return BadRequest<string>(ex.Message);
            }
            var complete = await unitOfWork.CompleteAsync() > 0;
            if (!complete) return BadRequest<string>("Error Occure While Deleting Event");
            return Success("Event Deleted Successfully");

        }


        public async Task<Pagination<EventResponse>> GetAllEventsAsynce(SpecParams specParams, CancellationToken cancellationToken = default)
        {


            var spec = new EventSpecification(specParams.Search, specParams.CategoryId, specParams.Sort, specParams.PageSize, specParams.PageIndex);
            var eventRepo = unitOfWork.GetRepository<Event, int>();
            var events = await eventRepo.GetAllWithSpecAsync(spec);
            var data = mapper.Map<IEnumerable<EventResponse>>(events);
            var countspec = new EventWithFiltrationCountSpecification(specParams.Search, specParams.CategoryId);
            var count = await eventRepo.GetCountAsync(spec, cancellationToken);
            return new Pagination<EventResponse>(specParams.PageIndex, specParams.PageSize, count) { Data = data };

        }

        public async Task<Response<EventResponse>> GetEventAsync(int id, CancellationToken cancellationToken = default)
        {
            var spec = new EventSpecification(id);
            var eventRepo = unitOfWork.GetRepository<Event, int>();
            var eventEntity = await eventRepo.GetWithSpecAsync(spec, cancellationToken);
            if (eventEntity is null)
            {
                return NotFound<EventResponse>(id, "Event Not Found With This Id");
            }
            var mappedEvent = mapper.Map<EventResponse>(eventEntity);
            return Success(mappedEvent, 1);

        }


    }
}
