using System.ComponentModel.DataAnnotations;

namespace BookEvent.Shared.Models.Auth
{
    public class ForgetPasswordByEmailDto
    {

        [Required]
        public required string Email { get; set; }
    }
}
