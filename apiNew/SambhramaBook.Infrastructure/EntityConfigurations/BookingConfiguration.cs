using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("bookings");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(b => b.BookingReference)
            .HasColumnName("booking_reference")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(b => b.CustomerId)
            .HasColumnName("customer_id")
            .IsRequired();

        builder.Property(b => b.VendorId)
            .HasColumnName("vendor_id")
            .IsRequired();

        builder.Property(b => b.ListingId)
            .HasColumnName("listing_id")
            .IsRequired();

        // Booking Details
        builder.Property(b => b.EventType)
            .HasColumnName("event_type")
            .HasMaxLength(100);

        builder.Property(b => b.EventName)
            .HasColumnName("event_name")
            .HasMaxLength(255);

        builder.Property(b => b.GuestCount)
            .HasColumnName("guest_count")
            .IsRequired();

        builder.Property(b => b.StartDate)
            .HasColumnName("start_date")
            .IsRequired();

        builder.Property(b => b.EndDate)
            .HasColumnName("end_date")
            .IsRequired();

        builder.Property(b => b.StartTime)
            .HasColumnName("start_time");

        builder.Property(b => b.EndTime)
            .HasColumnName("end_time");

        builder.Property(b => b.DurationDays)
            .HasColumnName("duration_days")
            .IsRequired();

        builder.Property(b => b.ServicePackageId)
            .HasColumnName("service_package_id");

        // Pricing
        builder.Property(b => b.BaseAmount)
            .HasColumnName("base_amount")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(b => b.DiscountAmount)
            .HasColumnName("discount_amount")
            .HasPrecision(10, 2)
            .HasDefaultValue(0);

        builder.Property(b => b.TaxAmount)
            .HasColumnName("tax_amount")
            .HasPrecision(10, 2)
            .HasDefaultValue(0);

        builder.Property(b => b.PlatformFee)
            .HasColumnName("platform_fee")
            .HasPrecision(10, 2)
            .HasDefaultValue(0);

        builder.Property(b => b.TotalAmount)
            .HasColumnName("total_amount")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(b => b.AmountPaid)
            .HasColumnName("amount_paid")
            .HasPrecision(10, 2)
            .HasDefaultValue(0);

        builder.Property(b => b.RefundAmount)
            .HasColumnName("refund_amount")
            .HasPrecision(10, 2)
            .HasDefaultValue(0);

        // Status
        builder.Property(b => b.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .HasDefaultValue(BookingStatus.Pending);

        builder.Property(b => b.PaymentStatus)
            .HasColumnName("payment_status")
            .HasConversion<int>()
            .HasDefaultValue(PaymentStatus.Pending);

        // Payment
        builder.Property(b => b.PaymentMethod)
            .HasColumnName("payment_method")
            .HasMaxLength(50);

        builder.Property(b => b.PaymentTransactionId)
            .HasColumnName("payment_transaction_id")
            .HasMaxLength(255);

        builder.Property(b => b.PaymentDate)
            .HasColumnName("payment_date");

        // Cancellation
        builder.Property(b => b.CancellationReason)
            .HasColumnName("cancellation_reason");

        builder.Property(b => b.CancellationFee)
            .HasColumnName("cancellation_fee")
            .HasPrecision(10, 2)
            .HasDefaultValue(0);

        builder.Property(b => b.CancelledAt)
            .HasColumnName("cancelled_at");

        builder.Property(b => b.CancelledBy)
            .HasColumnName("cancelled_by");

        // Special Requirements
        builder.Property(b => b.SpecialRequirements)
            .HasColumnName("special_requirements");

        builder.Property(b => b.AdditionalNotes)
            .HasColumnName("additional_notes");

        // Vendor Response
        builder.Property(b => b.VendorStatus)
            .HasColumnName("vendor_status")
            .HasMaxLength(20)
            .HasDefaultValue("PENDING");

        builder.Property(b => b.VendorResponseNotes)
            .HasColumnName("vendor_response_notes");

        builder.Property(b => b.VendorRespondedAt)
            .HasColumnName("vendor_responded_at");

        builder.Property(b => b.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(b => b.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(b => b.ConfirmedAt)
            .HasColumnName("confirmed_at");

        builder.Property(b => b.CompletedAt)
            .HasColumnName("completed_at");

        // Indexes
        builder.HasIndex(b => b.BookingReference).IsUnique();
        builder.HasIndex(b => b.CustomerId);
        builder.HasIndex(b => b.VendorId);
        builder.HasIndex(b => b.ListingId);
        builder.HasIndex(b => b.Status);
        builder.HasIndex(b => b.PaymentStatus);
        builder.HasIndex(b => new { b.StartDate, b.EndDate });
        builder.HasIndex(b => b.CreatedAt);

        // Relationships
        builder.HasOne(b => b.Customer)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Vendor)
            .WithMany(vp => vp.Bookings)
            .HasForeignKey(b => b.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Listing)
            .WithMany(l => l.Bookings)
            .HasForeignKey(b => b.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.ServicePackage)
            .WithMany()
            .HasForeignKey(b => b.ServicePackageId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

