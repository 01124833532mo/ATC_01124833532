using BookEvent.Core.Domain.Entities.Events;

namespace BookEvent.Core.Domain.Specifications.Events
{
    public class EventSpecification : BaseSpecification<Event, int>
    {

        public EventSpecification(string? search, int? categoryId, string? sort, int pageSize, int pageIndex)
            : base(


                  p =>
                  (string.IsNullOrEmpty(search) || p.NormalizedName.Contains(search))
                  &&
                  (!categoryId.HasValue || p.CategoryId == categoryId.Value)
                            &&
                        (!categoryId.HasValue || p.CategoryId == categoryId.Value)
                  )
        {
            AddIncludes();



            switch (sort)
            {
                case "nameDesc":
                    AddOrderByDesc(p => p.Name);
                    break;

                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;

                case "priceDesc":
                    AddOrderByDesc(p => p.Price);
                    break;

                default:
                    AddOrderByDesc(p => p.CreatedOn);
                    break;
            }

            // totalproducts 18 ~ 20
            //page size = 5
            //page index = 3

            ApplyPagination((pageIndex - 1) * pageSize, pageSize);


        }



        public EventSpecification(int id) : base(id)
        {
            AddIncludes();

        }

        private protected override void AddIncludes()
        {
            base.AddIncludes();
            Includes.Add(p => p.Category!);
        }
    }
}
