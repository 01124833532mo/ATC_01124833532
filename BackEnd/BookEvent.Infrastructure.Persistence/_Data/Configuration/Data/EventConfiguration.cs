using BookEvent.Core.Domain.Entities.Events;
using BookEvent.Infrastructure.Persistence._Data.Configuration.Base;
using Microsoft.EntityFrameworkCore;

namespace BookEvent.Infrastructure.Persistence._Data.Configuration.Data
{
    internal class EventConfiguration : BaseAuditableEntityConfigurations<Event, int>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Event> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => e.Id);
            builder.Property(p => p.Name)
                     .HasColumnType("nvarchar")
                     .HasMaxLength(50);
            builder.Property(p => p.Description)
                        .HasColumnType("nvarchar")
                        .HasMaxLength(500);

            builder.Property(p => p.Venue)
                        .HasColumnType("nvarchar")
                        .HasMaxLength(500);
            builder.Property(e => e.Price)
           .HasColumnType("decimal(18,2)")
           .IsRequired();

            builder.Property(e => e.ImagePath)
                .HasColumnType("nvarchar")
                .HasMaxLength(500);
            builder.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Books)
                    .WithOne(b => b.Event)
                    .HasForeignKey(b => b.EventId)
                    .OnDelete(DeleteBehavior.Cascade);




        }
    }

}
