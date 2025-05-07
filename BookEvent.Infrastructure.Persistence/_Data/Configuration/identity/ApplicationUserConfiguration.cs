using BookEvent.Core.Domain.Entities._Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookEvent.Infrastructure.Persistence._Data.Configuration.identity
{
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            builder.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(100);

        }
    }

}
