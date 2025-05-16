using BookEvent.Core.Domain.Common;
using BookEvent.Core.Domain.Entities._Identity;
using BookEvent.Core.Domain.Entities.Events;

namespace BookEvent.Core.Domain.Entities.Books
{
    public class Book : BaseAuditableEntity<int>
    {

        public int EventId { get; set; }

        public string UserId { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        public virtual Event Event { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
