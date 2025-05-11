using BookEvent.Core.Domain.Entities.Events;

namespace BookEvent.Core.Domain.Specifications.Events
{
    public class EventWithFiltrationCountSpecification : BaseSpecification<Event, int>
    {

        public EventWithFiltrationCountSpecification(string? search, int? categoryId)
            : base(
                   p =>
                                     (string.IsNullOrEmpty(search) || p.NormalizedName.Contains(search))
                                &&
                   (!categoryId.HasValue || p.CategoryId == categoryId.Value)
                            &&
                        (!categoryId.HasValue || p.CategoryId == categoryId.Value)
                 )
        {

        }

    }

}
