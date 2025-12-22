using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
{
    public void Configure(EntityTypeBuilder<Amenity> builder)
    {
        builder.ToTable(nameof(Amenity), DefaultDatabaseSchema.Name);

        builder.HasKey(a => a.Id)
            .HasName("PK_Amenity_Id");

        builder.Property(a => a.Id)
            .IsRequired();

        builder.Property(a => a.Code)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(a => a.DisplayName)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode(false);

        builder.Property(a => a.IconUrl)
            .HasMaxLength(500)
            .IsUnicode(false);

        builder.Property(a => a.Scope)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(a => a.IsActive)
            .IsRequired();

        builder.Property(a => a.DisplayOrder)
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(a => a.UpdatedAt)
            .IsRequired();

        builder.HasIndex(a => a.Code)
            .HasDatabaseName("IX_Amenity_Code")
            .IsUnique();

        builder.HasIndex(a => a.IsActive)
            .HasDatabaseName("IX_Amenity_IsActive");

        builder.HasIndex(a => new { a.Scope, a.IsActive, a.DisplayOrder })
            .HasDatabaseName("IX_Amenity_Scope_IsActive_DisplayOrder");
    }
}

