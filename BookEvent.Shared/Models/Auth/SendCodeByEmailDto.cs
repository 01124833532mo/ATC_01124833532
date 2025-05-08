using System.ComponentModel.DataAnnotations;

namespace BookEvent.Shared.Models.Auth
{
    public class SendCodeByEmailDto
    {

        [Required]
        public required string Email { get; set; }
    }
}
