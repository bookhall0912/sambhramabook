using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class PayoutConfiguration : IEntityTypeConfiguration<Payout>
{
    public void Configure(EntityTypeBuilder<Payout> builder)
    {
        builder.ToTable("payouts");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(p => p.VendorId)
            .HasColumnName("vendor_id")
            .IsRequired();

        builder.Property(p => p.BookingId)
            .HasColumnName("booking_id");

        builder.Property(p => p.Amount)
            .HasColumnName("amount")
            .HasPrecision(12, 2)
            .IsRequired();

        builder.Property(p => p.PlatformCommission)
            .HasColumnName("platform_commission")
            .HasPrecision(12, 2)
            .IsRequired();

        builder.Property(p => p.NetAmount)
            .HasColumnName("net_amount")
            .HasPrecision(12, 2)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasColumnName("status")
            .HasMaxLength(20)
            .HasDefaultValue("PENDING");

        builder.Property(p => p.PayoutMethod)
            .HasColumnName("payout_method")
            .HasMaxLength(50);

        builder.Property(p => p.TransactionReference)
            .HasColumnName("transaction_reference")
            .HasMaxLength(255);

        builder.Property(p => p.BankAccountId)
            .HasColumnName("bank_account_id");

        builder.Property(p => p.ProcessedAt)
            .HasColumnName("processed_at");

        builder.Property(p => p.ProcessedBy)
            .HasColumnName("processed_by");

        builder.Property(p => p.FailureReason)
            .HasColumnName("failure_reason");

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(p => p.VendorId);
        builder.HasIndex(p => p.BookingId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.CreatedAt);

        // Relationships
        builder.HasOne(p => p.Vendor)
            .WithMany()
            .HasForeignKey(p => p.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Booking)
            .WithMany()
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

