using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class PayoutConfiguration : IEntityTypeConfiguration<Payout>
{
    public void Configure(EntityTypeBuilder<Payout> builder)
    {
        builder.ToTable(nameof(Payout), DefaultDatabaseSchema.Name);

        builder.HasKey(p => p.Id)
            .HasName("PK_Payout_Id");

        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.VendorId)
            .IsRequired();

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.PayoutStatus)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.PayoutDate);

        builder.HasOne(p => p.Vendor)
            .WithMany(v => v.Payouts)
            .HasForeignKey(p => p.VendorId)
            .HasConstraintName($"FK_{nameof(Payout)}_{nameof(Vendor)}")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(p => p.VendorId)
            .HasDatabaseName("IX_Payout_VendorId");

        builder.HasIndex(p => new { p.PayoutStatus, p.PayoutDate })
            .HasDatabaseName("IX_Payout_PayoutStatus_PayoutDate");
    }
}

