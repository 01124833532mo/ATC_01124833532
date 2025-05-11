using AutoMapper;
using BookEvent.Core.Domain.Entities.Books;
using BookEvent.Shared.Models.Booking;
using Microsoft.Extensions.Configuration;

namespace BookEvent.Core.Application.Services.Mapping
{
    public class PictureOfEventINBoogingResolver(IConfiguration configuration) : IValueResolver<Book, BookToReturn, string?>
    {
        public string? Resolve(Book source, BookToReturn destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Event.ImagePath))
            {
                return $"{configuration["Urls:ApiBaseUrl"]}/{source.Event.ImagePath}";
            }
            return string.Empty;

        }
    }
}
