using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class ServicePackageConfiguration : IEntityTypeConfiguration<ServicePackage>
{
    public void Configure(EntityTypeBuilder<ServicePackage> builder)
    {
        builder.ToTable("service_packages");

        builder.HasKey(sp => sp.Id);
        builder.Property(sp => sp.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(sp => sp.ListingId)
            .HasColumnName("listing_id")
            .IsRequired();

        builder.Property(sp => sp.PackageName)
            .HasColumnName("package_name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(sp => sp.Description)
            .HasColumnName("description");

        builder.Property(sp => sp.Price)
            .HasColumnName("price")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(sp => sp.DurationHours)
            .HasColumnName("duration_hours");

        builder.Property(sp => sp.Includes)
            .HasColumnName("includes");

        builder.Property(sp => sp.DisplayOrder)
            .HasColumnName("display_order")
            .HasDefaultValue(0);

        builder.Property(sp => sp.IsPopular)
            .HasColumnName("is_popular")
            .HasDefaultValue(false);

        builder.Property(sp => sp.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(sp => sp.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(sp => sp.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(sp => sp.ListingId);
        builder.HasIndex(sp => new { sp.ListingId, sp.IsActive });

        // Relationships
        builder.HasOne(sp => sp.Listing)
            .WithMany(l => l.ServicePackages)
            .HasForeignKey(sp => sp.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

