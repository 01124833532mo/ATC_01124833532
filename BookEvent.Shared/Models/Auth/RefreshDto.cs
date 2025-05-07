using System.ComponentModel.DataAnnotations;

namespace BookEvent.Shared.Models.Auth
{
    public class RefreshDto
    {
        [Required]
        public required string Token { get; set; }
        [Required]

        public required string RefreshToken { get; set; }
    }
}
