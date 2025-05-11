using AutoMapper;
using BookEvent.Core.Domain.Entities.Categories;
using BookEvent.Core.Domain.Entities.Events;
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




        }
    }
}
