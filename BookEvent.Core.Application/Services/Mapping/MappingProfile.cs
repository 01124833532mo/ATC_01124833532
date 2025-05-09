using AutoMapper;
using BookEvent.Core.Domain.Entities.Categories;
using BookEvent.Shared.Models.Categories;

namespace BookEvent.Core.Application.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryToRetuen>();


        }
    }
}
