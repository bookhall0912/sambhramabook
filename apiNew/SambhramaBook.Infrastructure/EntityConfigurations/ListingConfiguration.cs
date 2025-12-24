using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class ListingConfiguration : IEntityTypeConfiguration<Listing>
{
    public void Configure(EntityTypeBuilder<Listing> builder)
    {
        builder.ToTable("listings");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(l => l.VendorId)
            .HasColumnName("vendor_id")
            .IsRequired();

        builder.Property(l => l.ListingType)
            .HasColumnName("listing_type")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(l => l.ServiceCategoryId)
            .HasColumnName("service_category_id");

        builder.Property(l => l.Title)
            .HasColumnName("title")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(l => l.Slug)
            .HasColumnName("slug")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(l => l.Description)
            .HasColumnName("description");

        builder.Property(l => l.ShortDescription)
            .HasColumnName("short_description")
            .HasMaxLength(500);

        // Location
        builder.Property(l => l.AddressLine1)
            .HasColumnName("address_line1")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(l => l.AddressLine2)
            .HasColumnName("address_line2")
            .HasMaxLength(255);

        builder.Property(l => l.City)
            .HasColumnName("city")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(l => l.State)
            .HasColumnName("state")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(l => l.Pincode)
            .HasColumnName("pincode")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(l => l.Country)
            .HasColumnName("country")
            .HasMaxLength(100)
            .HasDefaultValue("India");

        builder.Property(l => l.Latitude)
            .HasColumnName("latitude")
            .HasPrecision(10, 8);

        builder.Property(l => l.Longitude)
            .HasColumnName("longitude")
            .HasPrecision(11, 8);

        // For Halls
        builder.Property(l => l.CapacityMin)
            .HasColumnName("capacity_min");

        builder.Property(l => l.CapacityMax)
            .HasColumnName("capacity_max");

        builder.Property(l => l.AreaSqft)
            .HasColumnName("area_sqft");

        builder.Property(l => l.ParkingCapacity)
            .HasColumnName("parking_capacity");

        // Pricing
        builder.Property(l => l.BasePrice)
            .HasColumnName("base_price")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(l => l.PricePerHour)
            .HasColumnName("price_per_hour")
            .HasPrecision(10, 2);

        builder.Property(l => l.PricePerDay)
            .HasColumnName("price_per_day")
            .HasPrecision(10, 2);

        builder.Property(l => l.Currency)
            .HasColumnName("currency")
            .HasMaxLength(3)
            .HasDefaultValue("INR");

        builder.Property(l => l.CancellationPolicy)
            .HasColumnName("cancellation_policy");

        // Status
        builder.Property(l => l.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .HasDefaultValue(ListingStatus.Draft);

        builder.Property(l => l.ApprovalStatus)
            .HasColumnName("approval_status")
            .HasConversion<int>()
            .HasDefaultValue(ApprovalStatus.Pending);

        builder.Property(l => l.ApprovalNotes)
            .HasColumnName("approval_notes");

        builder.Property(l => l.ApprovedAt)
            .HasColumnName("approved_at");

        builder.Property(l => l.ApprovedBy)
            .HasColumnName("approved_by");

        // Statistics
        builder.Property(l => l.ViewCount)
            .HasColumnName("view_count")
            .HasDefaultValue(0);

        builder.Property(l => l.BookingCount)
            .HasColumnName("booking_count")
            .HasDefaultValue(0);

        builder.Property(l => l.AverageRating)
            .HasColumnName("average_rating")
            .HasPrecision(3, 2)
            .HasDefaultValue(0);

        builder.Property(l => l.TotalReviews)
            .HasColumnName("total_reviews")
            .HasDefaultValue(0);

        // SEO
        builder.Property(l => l.MetaTitle)
            .HasColumnName("meta_title")
            .HasMaxLength(255);

        builder.Property(l => l.MetaDescription)
            .HasColumnName("meta_description");

        builder.Property(l => l.MetaKeywords)
            .HasColumnName("meta_keywords");

        builder.Property(l => l.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(l => l.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(l => l.PublishedAt)
            .HasColumnName("published_at");

        builder.Property(l => l.DeletedAt)
            .HasColumnName("deleted_at");

        // Indexes
        builder.HasIndex(l => l.VendorId);
        builder.HasIndex(l => l.ListingType);
        builder.HasIndex(l => l.Status);
        builder.HasIndex(l => l.ApprovalStatus);
        builder.HasIndex(l => l.City);
        builder.HasIndex(l => l.State);
        builder.HasIndex(l => new { l.Latitude, l.Longitude });
        builder.HasIndex(l => l.Slug).IsUnique();

        // Relationships
        builder.HasOne(l => l.Vendor)
            .WithMany(vp => vp.Listings)
            .HasForeignKey(l => l.VendorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.ServiceCategory)
            .WithMany(sc => sc.Listings)
            .HasForeignKey(l => l.ServiceCategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(l => l.ServiceCategoryId);
    }
}

