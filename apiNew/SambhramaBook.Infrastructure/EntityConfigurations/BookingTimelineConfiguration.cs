using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class BookingTimelineConfiguration : IEntityTypeConfiguration<BookingTimeline>
{
    public void Configure(EntityTypeBuilder<BookingTimeline> builder)
    {
        builder.ToTable("booking_timelines");

        builder.HasKey(bt => bt.Id);
        builder.Property(bt => bt.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(bt => bt.BookingId)
            .HasColumnName("booking_id")
            .IsRequired();

        builder.Property(bt => bt.StatusFrom)
            .HasColumnName("status_from")
            .HasMaxLength(50);

        builder.Property(bt => bt.StatusTo)
            .HasColumnName("status_to")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(bt => bt.ChangedBy)
            .HasColumnName("changed_by");

        builder.Property(bt => bt.Notes)
            .HasColumnName("notes");

        builder.Property(bt => bt.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(bt => bt.BookingId);
        builder.HasIndex(bt => bt.CreatedAt);

        // Relationships
        builder.HasOne(bt => bt.Booking)
            .WithMany(b => b.Timeline)
            .HasForeignKey(bt => bt.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

