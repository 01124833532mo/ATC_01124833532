using BookEvent.Core.Application.Abstraction.Bases;
using BookEvent.Core.Application.Abstraction.Common;
using BookEvent.Shared.Models.Categories;

namespace BookEvent.Core.Application.Abstraction.Services.Categories
{
    public interface ICategoriesService
    {
        public Task<Response<CategoryToRetuen>> CreateCategory(CategoryDto categoryDto, CancellationToken cancellationToken = default);
        public Task<Response<CategoryToRetuen>> UpdateCategory(int id, CategoryDto categoryDto, CancellationToken cancellationToke = default);
        public Task<Response<CategoryToRetuen>> DeleteCategory(int id, CancellationToken cancellationToken = default);
        public Task<Response<CategoryDto>> GetCategoryAsync(int id, CancellationToken cancellationToken = default);

        Task<Pagination<CategoryToRetuen>> GetAllCategoriesAsynce(SpecParams specParams, CancellationToken cancellationToken = default);

    }
}
