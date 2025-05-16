using BookEvent.Core.Domain.Entities.Books;
using Microsoft.AspNetCore.Identity;

namespace BookEvent.Core.Domain.Entities._Identity
{
    public class ApplicationUser : IdentityUser
    {
        public required string FullName { get; set; }

        public int? ResetCode { get; set; }
        public DateTime? ResetCodeExpiry { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public virtual ICollection<Book>? Books { get; set; }


    }
}
