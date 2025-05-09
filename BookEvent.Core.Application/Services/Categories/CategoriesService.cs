using AutoMapper;
using BookEvent.Core.Application.Abstraction.Bases;
using BookEvent.Core.Application.Abstraction.Services.Categories;
using BookEvent.Core.Domain.Contracts.Persestence;
using BookEvent.Core.Domain.Entities.Categories;
using BookEvent.Shared.Models.Categories;

namespace BookEvent.Core.Application.Services.Categories
{
    public class CategoriesService(IUnitOfWork unitOfWork, IMapper mapper) : ResponseHandler, ICategoriesService
    {
        public async Task<Response<CategoryToRetuen>> CreateCategory(CategoryDto categoryDto, CancellationToken cancellationToken)
        {
            var categoryrepo = unitOfWork.GetRepository<Category, int>();
            var category = mapper.Map<Category>(categoryDto);

            await categoryrepo.AddAsync(category);

            var compelete = await unitOfWork.CompleteAsync() > 0;

            if (!compelete) return BadRequest<CategoryToRetuen>("Error Occure While Creating Category");

            var mappedresult = mapper.Map<CategoryToRetuen>(category);


            return Created(mappedresult);






        }
    }
}
