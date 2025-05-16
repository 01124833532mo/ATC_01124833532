using System.ComponentModel.DataAnnotations;

namespace BookEvent.Shared.Models.Booking
{
    public class CreateBookDto
    {
        [Required]
        public int EventId { get; set; }
    }
}
