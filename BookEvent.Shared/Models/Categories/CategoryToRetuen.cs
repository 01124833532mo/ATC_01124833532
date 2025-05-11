namespace BookEvent.Shared.Models.Categories
{
    public class CategoryToRetuen
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; } = null!;


        public DateTime LastModifiedOn { get; set; }

    }
}
