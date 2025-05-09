using BookEvent.Core.Application.Abstraction.Bases;
using BookEvent.Shared.Models.Categories;

namespace BookEvent.Core.Application.Abstraction.Services.Categories
{
    public interface ICategoriesService
    {
        public Task<Response<CategoryToRetuen>> CreateCategory(CategoryDto categoryDto, CancellationToken cancellationToken = default);
        public Task<Response<CategoryToRetuen>> UpdateCategory(int id, CategoryDto categoryDto, CancellationToken cancellationToke = default);
        public Task<Response<CategoryToRetuen>> DeleteCategory(int id, CancellationToken cancellationToken = default);

    }
}
