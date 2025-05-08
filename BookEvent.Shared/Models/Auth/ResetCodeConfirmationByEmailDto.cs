using System.ComponentModel.DataAnnotations;

namespace BookEvent.Shared.Models.Auth
{
    public class ResetCodeConfirmationByEmailDto : ForgetPasswordByEmailDto
    {
        [Required]
        public required int ResetCode { get; set; }
    }
}
