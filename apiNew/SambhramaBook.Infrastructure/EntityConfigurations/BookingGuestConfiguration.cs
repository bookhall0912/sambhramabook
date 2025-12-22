using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class BookingGuestConfiguration : IEntityTypeConfiguration<BookingGuest>
{
    public void Configure(EntityTypeBuilder<BookingGuest> builder)
    {
        builder.ToTable("booking_guests");

        builder.HasKey(bg => bg.Id);
        builder.Property(bg => bg.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(bg => bg.BookingId)
            .HasColumnName("booking_id")
            .IsRequired();

        builder.Property(bg => bg.GuestName)
            .HasColumnName("guest_name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(bg => bg.GuestEmail)
            .HasColumnName("guest_email")
            .HasMaxLength(255);

        builder.Property(bg => bg.GuestPhone)
            .HasColumnName("guest_phone")
            .HasMaxLength(15);

        builder.Property(bg => bg.IsPrimaryContact)
            .HasColumnName("is_primary_contact")
            .HasDefaultValue(false);

        builder.Property(bg => bg.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(bg => bg.BookingId);

        // Relationships
        builder.HasOne(bg => bg.Booking)
            .WithMany(b => b.Guests)
            .HasForeignKey(bg => bg.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

