using AutoMapper;
using BookEvent.Core.Application.Abstraction.Bases;
using BookEvent.Core.Application.Abstraction.Common;
using BookEvent.Core.Application.Abstraction.Services.Categories;
using BookEvent.Core.Domain.Contracts.Persestence;
using BookEvent.Core.Domain.Entities.Categories;
using BookEvent.Core.Domain.Specifications.Categories;
using BookEvent.Shared.Models.Categories;
using Microsoft.EntityFrameworkCore;

namespace BookEvent.Core.Application.Services.Categories
{
    public class CategoriesService(IUnitOfWork unitOfWork, IMapper mapper) : ResponseHandler, ICategoriesService
    {
        public async Task<Response<CategoryToRetuen>> CreateCategory(CategoryDto categoryDto, CancellationToken cancellationToken)
        {
            var categoryrepo = unitOfWork.GetRepository<Category, int>();
            var category = mapper.Map<Category>(categoryDto);

            try
            {
                await categoryrepo.AddAsync(category);

            }
            catch (Exception ex)
            {
                return BadRequest<CategoryToRetuen>(ex.Message);
            }
            var compelete = await unitOfWork.CompleteAsync() > 0;

            if (!compelete) return BadRequest<CategoryToRetuen>("Error Occure While Creating Category");

            var mappedresult = mapper.Map<CategoryToRetuen>(category);


            return Created(mappedresult);


        }



        public async Task<Response<CategoryToRetuen>> UpdateCategory(int id, CategoryDto categoryDto, CancellationToken cancellationToke = default)
        {
            var categoryrepo = unitOfWork.GetRepository<Category, int>();
            var category = await categoryrepo.GetAsync(id, cancellationToke);
            if (category is null) return NotFound<CategoryToRetuen>("Category Not Found");
            mapper.Map(categoryDto, category);
            try
            {
                categoryrepo.Update(category);

            }
            catch (Exception ex)
            {
                return BadRequest<CategoryToRetuen>(ex.Message);
            }
            var compelete = await unitOfWork.CompleteAsync() > 0;
            if (!compelete) return BadRequest<CategoryToRetuen>("Error Occure While Updating Category");
            var mappedresult = mapper.Map<CategoryToRetuen>(category);
            return Success(mappedresult);
        }

        public async Task<Response<CategoryToRetuen>> DeleteCategory(int id, CancellationToken cancellationToken = default)
        {
            var categoryrepo = unitOfWork.GetRepository<Category, int>();
            var category = await categoryrepo.GetAsync(id, cancellationToken);
            if (category is null) return NotFound<CategoryToRetuen>("Category Not Found");
            try
            {
                categoryrepo.Delete(category);
            }
            catch (Exception ex)
            {
                return BadRequest<CategoryToRetuen>(ex.Message);
            }
            var compelete = await unitOfWork.CompleteAsync() > 0;
            if (!compelete) return BadRequest<CategoryToRetuen>("Error Occure While Deleting Category");
            var mappedresult = mapper.Map<CategoryToRetuen>(category);
            return Deleted<CategoryToRetuen>();
        }

        public async Task<Response<CategoryDto>> GetCategoryAsync(int id, CancellationToken cancellationToken = default)
        {
            var repo = unitOfWork.GetRepository<Category, int>();
            var category = await repo.GetAsync(id, cancellationToken);

            if (category is null)
            {
                return NotFound<CategoryDto>(id, "Category Not Found With This Id");
            }

            var mappedCategory = mapper.Map<CategoryDto>(category);

            return Success(mappedCategory, 1);

        }

        public async Task<Pagination<CategoryToRetuen>> GetAllCategoriesAsynce(SpecParams specParams, CancellationToken cancellationToken = default)
        {
            var spec = new CategoriesSpecification(specParams.PageSize, specParams.PageIndex);

            var repo = unitOfWork.GetRepository<Category, int>();

            var Categories = await repo.GetAllWithSpecAsync(spec);

            var data = mapper.Map<IEnumerable<CategoryToRetuen>>(Categories);
            var count = await repo.GetQuarable().CountAsync();

            return new Pagination<CategoryToRetuen>(specParams.PageIndex, specParams.PageSize, count) { Data = data };
        }
    }
}
