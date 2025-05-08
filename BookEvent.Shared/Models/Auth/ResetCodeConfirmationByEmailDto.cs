using System.ComponentModel.DataAnnotations;

namespace BookEvent.Shared.Models.Auth
{
    public class ResetCodeConfirmationByEmailDto : SendCodeByEmailDto
    {
        [Required]
        public required int ResetCode { get; set; }
    }
}
