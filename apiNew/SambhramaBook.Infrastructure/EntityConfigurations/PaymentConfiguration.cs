using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(p => p.BookingId)
            .HasColumnName("booking_id")
            .IsRequired();

        builder.Property(p => p.PaymentReference)
            .HasColumnName("payment_reference")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Amount)
            .HasColumnName("amount")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(p => p.Currency)
            .HasColumnName("currency")
            .HasMaxLength(3)
            .HasDefaultValue("INR");

        builder.Property(p => p.PaymentMethod)
            .HasColumnName("payment_method")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.PaymentGateway)
            .HasColumnName("payment_gateway")
            .HasMaxLength(50);

        builder.Property(p => p.GatewayTransactionId)
            .HasColumnName("gateway_transaction_id")
            .HasMaxLength(255);

        builder.Property(p => p.GatewayResponse)
            .HasColumnName("gateway_response");

        builder.Property(p => p.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .HasDefaultValue(PaymentStatus.Pending);

        builder.Property(p => p.FailureReason)
            .HasColumnName("failure_reason");

        builder.Property(p => p.PaidAt)
            .HasColumnName("paid_at");

        builder.Property(p => p.RefundedAt)
            .HasColumnName("refunded_at");

        builder.Property(p => p.RefundAmount)
            .HasColumnName("refund_amount")
            .HasPrecision(10, 2)
            .HasDefaultValue(0);

        builder.Property(p => p.RefundReason)
            .HasColumnName("refund_reason");

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(p => p.BookingId);
        builder.HasIndex(p => p.PaymentReference).IsUnique();
        builder.HasIndex(p => p.GatewayTransactionId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.CreatedAt);

        // Relationships
        builder.HasOne(p => p.Booking)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

