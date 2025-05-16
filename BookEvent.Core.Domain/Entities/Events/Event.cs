using BookEvent.Core.Domain.Common;
using BookEvent.Core.Domain.Entities.Books;
using BookEvent.Core.Domain.Entities.Categories;

namespace BookEvent.Core.Domain.Entities.Events
{
    public class Event : BaseAuditableEntity<int>
    {

        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public string Description { get; set; }

        public int? CategoryId { get; set; }
        public DateTime Date { get; set; }

        public string Venue { get; set; }


        public decimal Price { get; set; }

        public string? ImagePath { get; set; }


        public virtual ICollection<Book>? Books { get; set; }
        public virtual Category Category { get; set; }

    }
}
