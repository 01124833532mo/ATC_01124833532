using BookEvent.Core.Domain.Common;
using BookEvent.Core.Domain.Entities.Events;

namespace BookEvent.Core.Domain.Entities.Categories
{
    public class Category : BaseAuditableEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }


        //navigation property
        public virtual ICollection<Event>? Events { get; set; }
    }
}
