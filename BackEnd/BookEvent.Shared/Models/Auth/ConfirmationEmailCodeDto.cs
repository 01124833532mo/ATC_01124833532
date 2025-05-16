using System.ComponentModel.DataAnnotations;

namespace BookEvent.Shared.Models.Auth
{
    public class ConfirmationEmailCodeDto : SendCodeByEmailDto
    {


        [Required]
        public int ConfirmationCode { get; set; }
    }
}
