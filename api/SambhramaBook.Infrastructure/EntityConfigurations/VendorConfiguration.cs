using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.ToTable(nameof(Vendor), DefaultDatabaseSchema.Name);

        builder.HasKey(v => v.Id)
            .HasName("PK_Vendor_Id");

        builder.Property(v => v.Id)
            .IsRequired();

        builder.Property(v => v.UserId)
            .IsRequired();

        builder.Property(v => v.BusinessName)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode(false);

        builder.Property(v => v.VerificationStatus)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(v => v.CreatedAt)
            .IsRequired();

        builder.HasOne(v => v.User)
            .WithOne(u => u.Vendor)
            .HasForeignKey<Vendor>(v => v.UserId)
            .HasConstraintName($"FK_{nameof(Vendor)}_{nameof(User)}")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(v => v.UserId)
            .IsUnique()
            .HasDatabaseName("IX_Vendor_UserId");
    }
}

