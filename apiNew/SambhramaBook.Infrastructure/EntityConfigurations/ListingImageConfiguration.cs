using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class ListingImageConfiguration : IEntityTypeConfiguration<ListingImage>
{
    public void Configure(EntityTypeBuilder<ListingImage> builder)
    {
        builder.ToTable("listing_images");

        builder.HasKey(li => li.Id);
        builder.Property(li => li.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(li => li.ListingId)
            .HasColumnName("listing_id")
            .IsRequired();

        builder.Property(li => li.ImageUrl)
            .HasColumnName("image_url")
            .IsRequired();

        builder.Property(li => li.ImageType)
            .HasColumnName("image_type")
            .HasMaxLength(50)
            .HasDefaultValue("GALLERY");

        builder.Property(li => li.DisplayOrder)
            .HasColumnName("display_order")
            .HasDefaultValue(0);

        builder.Property(li => li.AltText)
            .HasColumnName("alt_text")
            .HasMaxLength(255);

        builder.Property(li => li.IsPrimary)
            .HasColumnName("is_primary")
            .HasDefaultValue(false);

        builder.Property(li => li.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(li => li.ListingId);
        builder.HasIndex(li => new { li.ListingId, li.IsPrimary });

        // Relationships
        builder.HasOne(li => li.Listing)
            .WithMany(l => l.Images)
            .HasForeignKey(li => li.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

