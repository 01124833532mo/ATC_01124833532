using Microsoft.AspNetCore.Http;

namespace BookEvent.Shared.Models.Events
{
    public class EventDto
    {
        public string Name { get; set; }
        public string NormalizedName { get { return Name.ToUpper(); } }

        public string Description { get; set; }

        public int? CategoryId { get; set; }
        public DateTime Date { get; set; }

        public string Venue { get; set; }


        public decimal Price { get; set; }

        public IFormFile? ImagePath { get; set; }
    }
}
