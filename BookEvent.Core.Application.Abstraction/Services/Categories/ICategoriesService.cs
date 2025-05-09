using BookEvent.Core.Application.Abstraction.Bases;
using BookEvent.Shared.Models.Categories;

namespace BookEvent.Core.Application.Abstraction.Services.Categories
{
    public interface ICategoriesService
    {
        public Task<Response<CategoryToRetuen>> CreateCategory(CategoryDto categoryDto, CancellationToken cancellationToken);

    }
}
