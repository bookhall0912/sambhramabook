using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class ServiceCategoryConfiguration : IEntityTypeConfiguration<ServiceCategory>
{
    public void Configure(EntityTypeBuilder<ServiceCategory> builder)
    {
        builder.ToTable(nameof(ServiceCategory), DefaultDatabaseSchema.Name);

        builder.HasKey(sc => sc.Id)
            .HasName("PK_ServiceCategory_Id");

        builder.Property(sc => sc.Id)
            .IsRequired();

        builder.Property(sc => sc.Code)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(sc => sc.DisplayName)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode(false);

        builder.Property(sc => sc.Description)
            .HasMaxLength(500)
            .IsUnicode(false);

        builder.Property(sc => sc.IconUrl)
            .HasMaxLength(500)
            .IsUnicode(false);

        builder.Property(sc => sc.BackgroundImageUrl)
            .HasMaxLength(500)
            .IsUnicode(false);

        builder.Property(sc => sc.ThemeColor)
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(sc => sc.IsActive)
            .IsRequired();

        builder.Property(sc => sc.DisplayOrder)
            .IsRequired();

        builder.HasIndex(sc => sc.Code)
            .HasDatabaseName("IX_ServiceCategory_Code")
            .IsUnique();

        builder.HasIndex(sc => sc.IsActive)
            .HasDatabaseName("IX_ServiceCategory_IsActive");

        builder.HasIndex(sc => sc.DisplayOrder)
            .HasDatabaseName("IX_ServiceCategory_DisplayOrder");
    }
}
