using System.ComponentModel.DataAnnotations;

namespace BookEvent.Shared.Models.Auth
{
    public class ResetPasswordByEmailDto : ForgetPasswordByEmailDto
    {
        [Required]
        public required string NewPassword { get; set; }
    }
}
