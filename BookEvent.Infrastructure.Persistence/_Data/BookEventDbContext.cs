using BookEvent.Core.Domain.Entities._Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookEvent.Infrastructure.Persistence._Data
{
    public class BookEventDbContext : IdentityDbContext<ApplicationUser>
    {
        public BookEventDbContext(DbContextOptions<BookEventDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AssemblyInformation).Assembly);
        }

    }
}
