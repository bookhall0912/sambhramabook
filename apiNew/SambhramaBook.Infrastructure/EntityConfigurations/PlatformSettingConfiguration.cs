using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class PlatformSettingConfiguration : IEntityTypeConfiguration<PlatformSetting>
{
    public void Configure(EntityTypeBuilder<PlatformSetting> builder)
    {
        builder.ToTable("platform_settings");

        builder.HasKey(ps => ps.Id);
        builder.Property(ps => ps.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(ps => ps.SettingKey)
            .HasColumnName("setting_key")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(ps => ps.SettingValue)
            .HasColumnName("setting_value")
            .IsRequired();

        builder.Property(ps => ps.SettingType)
            .HasColumnName("setting_type")
            .HasMaxLength(20)
            .HasDefaultValue("STRING");

        builder.Property(ps => ps.Description)
            .HasColumnName("description");

        builder.Property(ps => ps.Category)
            .HasColumnName("category")
            .HasMaxLength(50);

        builder.Property(ps => ps.IsPublic)
            .HasColumnName("is_public")
            .HasDefaultValue(false);

        builder.Property(ps => ps.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(ps => ps.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(ps => ps.SettingKey).IsUnique();
        builder.HasIndex(ps => ps.Category);
        builder.HasIndex(ps => ps.IsPublic);
    }
}

