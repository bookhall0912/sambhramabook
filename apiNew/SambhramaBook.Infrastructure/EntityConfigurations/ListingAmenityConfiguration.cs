using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class ListingAmenityConfiguration : IEntityTypeConfiguration<ListingAmenity>
{
    public void Configure(EntityTypeBuilder<ListingAmenity> builder)
    {
        builder.ToTable("listing_amenities");

        builder.HasKey(la => la.Id);
        builder.Property(la => la.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(la => la.ListingId)
            .HasColumnName("listing_id")
            .IsRequired();

        builder.Property(la => la.AmenityName)
            .HasColumnName("amenity_name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(la => la.AmenityCategory)
            .HasColumnName("amenity_category")
            .HasMaxLength(50);

        builder.Property(la => la.IconUrl)
            .HasColumnName("icon_url");

        builder.Property(la => la.IsAvailable)
            .HasColumnName("is_available")
            .HasDefaultValue(true);

        builder.Property(la => la.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(la => la.ListingId);
        builder.HasIndex(la => la.AmenityCategory);

        // Relationships
        builder.HasOne(la => la.Listing)
            .WithMany(l => l.Amenities)
            .HasForeignKey(la => la.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

