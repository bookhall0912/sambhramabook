using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable(nameof(Booking), DefaultDatabaseSchema.Name);

        builder.HasKey(b => b.Id)
            .HasName("PK_Booking_Id");

        builder.Property(b => b.Id)
            .IsRequired();

        builder.Property(b => b.BookingReference)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(b => b.UserId)
            .IsRequired();

        builder.Property(b => b.VendorId)
            .IsRequired();

        builder.Property(b => b.ServiceId)
            .IsRequired();

        builder.Property(b => b.StartDate)
            .IsRequired();

        builder.Property(b => b.EndDate)
            .IsRequired();

        builder.Property(b => b.Quantity)
            .IsRequired();

        builder.Property(b => b.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(b => b.TotalAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(b => b.CreatedAt)
            .IsRequired();

        builder.HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .HasConstraintName($"FK_{nameof(Booking)}_{nameof(User)}")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.Vendor)
            .WithMany(v => v.Bookings)
            .HasForeignKey(b => b.VendorId)
            .HasConstraintName($"FK_{nameof(Booking)}_{nameof(Vendor)}")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.Service)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.ServiceId)
            .HasConstraintName($"FK_{nameof(Booking)}_{nameof(Service)}")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(b => b.BookingReference)
            .IsUnique()
            .HasDatabaseName("IX_Booking_BookingReference");

        builder.HasIndex(b => b.UserId)
            .HasDatabaseName("IX_Booking_UserId");

        builder.HasIndex(b => b.VendorId)
            .HasDatabaseName("IX_Booking_VendorId");

        builder.HasIndex(b => b.ServiceId)
            .HasDatabaseName("IX_Booking_ServiceId");

        builder.HasIndex(b => new { b.Status, b.CreatedAt })
            .HasDatabaseName("IX_Booking_Status_CreatedAt");
    }
}

