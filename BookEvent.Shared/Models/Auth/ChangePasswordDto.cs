namespace BookEvent.Shared.Models.Auth
{
    public class ChangePasswordDto
    {
        public required string CurrentPassword { get; set; }

        public required string NewPassword { get; set; }
    }
}
