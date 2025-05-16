using BookEvent.Core.Domain.Entities.Categories;

namespace BookEvent.Core.Domain.Specifications.Categories
{
    public class CategoriesSpecification : BaseSpecification<Category, int>
    {
        public CategoriesSpecification(int pageSize, int pageIndex) : base()
        {
            ApplyPagination((pageIndex - 1) * pageSize, pageSize);

        }
    }
}
