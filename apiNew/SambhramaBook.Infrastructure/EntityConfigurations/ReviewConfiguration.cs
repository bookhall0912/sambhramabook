using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(r => r.BookingId)
            .HasColumnName("booking_id")
            .IsRequired();

        builder.Property(r => r.ListingId)
            .HasColumnName("listing_id")
            .IsRequired();

        builder.Property(r => r.CustomerId)
            .HasColumnName("customer_id")
            .IsRequired();

        builder.Property(r => r.VendorId)
            .HasColumnName("vendor_id")
            .IsRequired();

        builder.Property(r => r.Rating)
            .HasColumnName("rating")
            .IsRequired();

        builder.Property(r => r.Title)
            .HasColumnName("title")
            .HasMaxLength(255);

        builder.Property(r => r.Comment)
            .HasColumnName("comment");

        builder.Property(r => r.CleanlinessRating)
            .HasColumnName("cleanliness_rating");

        builder.Property(r => r.ServiceRating)
            .HasColumnName("service_rating");

        builder.Property(r => r.ValueRating)
            .HasColumnName("value_rating");

        builder.Property(r => r.LocationRating)
            .HasColumnName("location_rating");

        builder.Property(r => r.IsVerifiedBooking)
            .HasColumnName("is_verified_booking")
            .HasDefaultValue(true);

        builder.Property(r => r.IsPublished)
            .HasColumnName("is_published")
            .HasDefaultValue(true);

        builder.Property(r => r.IsHelpfulCount)
            .HasColumnName("is_helpful_count")
            .HasDefaultValue(0);

        builder.Property(r => r.VendorResponse)
            .HasColumnName("vendor_response");

        builder.Property(r => r.VendorRespondedAt)
            .HasColumnName("vendor_responded_at");

        builder.Property(r => r.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(r => r.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(r => r.BookingId).IsUnique();
        builder.HasIndex(r => r.ListingId);
        builder.HasIndex(r => r.CustomerId);
        builder.HasIndex(r => r.VendorId);
        builder.HasIndex(r => r.Rating);
        builder.HasIndex(r => r.IsPublished);
        builder.HasIndex(r => r.CreatedAt);

        // Relationships
        builder.HasOne(r => r.Booking)
            .WithOne(b => b.Review)
            .HasForeignKey<Review>(r => r.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Listing)
            .WithMany(l => l.Reviews)
            .HasForeignKey(r => r.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Customer)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Vendor)
            .WithMany()
            .HasForeignKey(r => r.VendorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

