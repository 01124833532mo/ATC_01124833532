using AutoMapper;
using BookEvent.Core.Domain.Entities.Books;
using BookEvent.Core.Domain.Entities.Categories;
using BookEvent.Core.Domain.Entities.Events;
using BookEvent.Shared.Models.Booking;
using BookEvent.Shared.Models.Categories;
using BookEvent.Shared.Models.Events;

namespace BookEvent.Core.Application.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<Category, CategoryToRetuen>();

            CreateMap<EventDto, Event>().ReverseMap();

            CreateMap<Event, EventResponse>()
                 .ForMember(dest => dest.ImagePath, opt => opt.MapFrom<EventPictureUrlResolver>())
                 .ForMember(dest => dest.CategoryName, otp => otp.MapFrom(src => src.Category.Name))
               .ForMember(dest => dest.CategoryDescription, otp => otp.MapFrom(src => src.Description));

            CreateMap<CreateBookDto, Book>().ReverseMap();
            CreateMap<Book, BookToReturn>()
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.Name))
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.Event.Id))
                .ForMember(dest => dest.CategoryName, otp => otp.MapFrom(src => src.Event.Category.Name))
                .ForMember(dest => dest.CategoryDescription, otp => otp.MapFrom(src => src.Event.Category.Description))
                .ForMember(dest => dest.EventDescription, opt => opt.MapFrom(src => src.Event.Description))
                .ForMember(dest => dest.EventVenue, opt => opt.MapFrom(src => src.Event.Venue))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom<PictureOfEventINBoogingResolver>())
                .ForMember(dest => dest.EventPrice, opt => opt.MapFrom(src => src.Event.Price))
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.BookingDate))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName));







        }
    }
}
