namespace BookEvent.Shared.Models.Events
{
    public class EventResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public DateTime Date { get; set; }
        public string Venue { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

    }
}
