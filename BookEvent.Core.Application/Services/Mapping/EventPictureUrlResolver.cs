using AutoMapper;
using BookEvent.Core.Domain.Entities.Events;
using BookEvent.Shared.Models.Events;
using Microsoft.Extensions.Configuration;

namespace BookEvent.Core.Application.Services.Mapping
{
    public class EventPictureUrlResolver(IConfiguration configuration) : IValueResolver<Event, EventResponse, string?>
    {
        public string? Resolve(Event source, EventResponse destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImagePath))
            {
                return $"{configuration["Urls:BookEventUrl"]}/{source.ImagePath}";
            }
            return string.Empty;

        }
    }
}
