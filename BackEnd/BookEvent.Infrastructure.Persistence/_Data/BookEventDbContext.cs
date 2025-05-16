using BookEvent.Core.Domain.Entities._Identity;
using BookEvent.Core.Domain.Entities.Books;
using BookEvent.Core.Domain.Entities.Categories;
using BookEvent.Core.Domain.Entities.Events;
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

        public DbSet<Book> Books { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
