using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class CommissionConfiguration : IEntityTypeConfiguration<Commission>
{
    public void Configure(EntityTypeBuilder<Commission> builder)
    {
        builder.ToTable(nameof(Commission), DefaultDatabaseSchema.Name);

        builder.HasKey(c => c.Id)
            .HasName("PK_Commission_Id");

        builder.Property(c => c.Id)
            .IsRequired();

        builder.Property(c => c.BookingId)
            .IsRequired();

        builder.Property(c => c.PlatformFee)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(c => c.VendorEarnings)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne(c => c.Booking)
            .WithOne(b => b.Commission)
            .HasForeignKey<Commission>(c => c.BookingId)
            .HasConstraintName($"FK_{nameof(Commission)}_{nameof(Booking)}")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(c => c.BookingId)
            .IsUnique()
            .HasDatabaseName("IX_Commission_BookingId");
    }
}

