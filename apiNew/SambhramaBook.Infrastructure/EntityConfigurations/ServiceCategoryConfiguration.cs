using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class ServiceCategoryConfiguration : IEntityTypeConfiguration<ServiceCategory>
{
    public void Configure(EntityTypeBuilder<ServiceCategory> builder)
    {
        builder.ToTable("service_categories");

        builder.HasKey(sc => sc.Id);
        builder.Property(sc => sc.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(sc => sc.Code)
            .HasColumnName("code")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(sc => sc.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(sc => sc.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder.Property(sc => sc.IconUrl)
            .HasColumnName("icon_url")
            .HasMaxLength(500);

        builder.Property(sc => sc.BackgroundImageUrl)
            .HasColumnName("background_image_url")
            .HasMaxLength(500);

        builder.Property(sc => sc.ThemeColor)
            .HasColumnName("theme_color")
            .HasMaxLength(20);

        builder.Property(sc => sc.DisplayOrder)
            .HasColumnName("display_order")
            .HasDefaultValue(0);

        builder.Property(sc => sc.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(sc => sc.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(sc => sc.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(sc => sc.Code).IsUnique();
        builder.HasIndex(sc => sc.DisplayOrder);
        builder.HasIndex(sc => sc.IsActive);
    }
}

