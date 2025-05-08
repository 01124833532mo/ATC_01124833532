namespace BookEvent.Shared.Models.Auth
{
    public class UpdateUserDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
