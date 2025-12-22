using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable(nameof(Payment), DefaultDatabaseSchema.Name);

        builder.HasKey(p => p.Id)
            .HasName("PK_Payment_Id");

        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.BookingId)
            .IsRequired();

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(p => p.PaymentStatus)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.TransactionReference)
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.HasOne(p => p.Booking)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BookingId)
            .HasConstraintName($"FK_{nameof(Payment)}_{nameof(Booking)}")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(p => p.BookingId)
            .HasDatabaseName("IX_Payment_BookingId");

        builder.HasIndex(p => p.TransactionReference)
            .HasDatabaseName("IX_Payment_TransactionReference")
            .HasFilter("[TransactionReference] IS NOT NULL");
    }
}

