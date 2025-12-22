using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class VendorProfileConfiguration : IEntityTypeConfiguration<VendorProfile>
{
    public void Configure(EntityTypeBuilder<VendorProfile> builder)
    {
        builder.ToTable("vendor_profiles");

        builder.HasKey(vp => vp.Id);
        builder.Property(vp => vp.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(vp => vp.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(vp => vp.BusinessName)
            .HasColumnName("business_name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(vp => vp.BusinessType)
            .HasColumnName("business_type")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(vp => vp.BusinessEmail)
            .HasColumnName("business_email")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(vp => vp.BusinessPhone)
            .HasColumnName("business_phone")
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(vp => vp.BusinessLogoUrl)
            .HasColumnName("business_logo_url");

        builder.Property(vp => vp.BusinessDescription)
            .HasColumnName("business_description");

        // Address
        builder.Property(vp => vp.AddressLine1)
            .HasColumnName("address_line1")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(vp => vp.AddressLine2)
            .HasColumnName("address_line2")
            .HasMaxLength(255);

        builder.Property(vp => vp.City)
            .HasColumnName("city")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(vp => vp.State)
            .HasColumnName("state")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(vp => vp.Pincode)
            .HasColumnName("pincode")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(vp => vp.Country)
            .HasColumnName("country")
            .HasMaxLength(100)
            .HasDefaultValue("India");

        builder.Property(vp => vp.Latitude)
            .HasColumnName("latitude")
            .HasPrecision(10, 8);

        builder.Property(vp => vp.Longitude)
            .HasColumnName("longitude")
            .HasPrecision(11, 8);

        // Business Details
        builder.Property(vp => vp.GstNumber)
            .HasColumnName("gst_number")
            .HasMaxLength(15);

        builder.Property(vp => vp.PanNumber)
            .HasColumnName("pan_number")
            .HasMaxLength(10);

        builder.Property(vp => vp.BankAccountNumber)
            .HasColumnName("bank_account_number")
            .HasMaxLength(50);

        builder.Property(vp => vp.IfscCode)
            .HasColumnName("ifsc_code")
            .HasMaxLength(11);

        builder.Property(vp => vp.BankName)
            .HasColumnName("bank_name")
            .HasMaxLength(255);

        builder.Property(vp => vp.AccountHolderName)
            .HasColumnName("account_holder_name")
            .HasMaxLength(255);

        // Status
        builder.Property(vp => vp.ProfileComplete)
            .HasColumnName("profile_complete")
            .HasDefaultValue(false);

        builder.Property(vp => vp.IsVerified)
            .HasColumnName("is_verified")
            .HasDefaultValue(false);

        builder.Property(vp => vp.VerificationStatus)
            .HasColumnName("verification_status")
            .HasConversion<int>()
            .HasDefaultValue(VerificationStatus.Pending);

        builder.Property(vp => vp.VerificationNotes)
            .HasColumnName("verification_notes");

        builder.Property(vp => vp.VerifiedAt)
            .HasColumnName("verified_at");

        builder.Property(vp => vp.VerifiedBy)
            .HasColumnName("verified_by");

        // Statistics
        builder.Property(vp => vp.TotalListings)
            .HasColumnName("total_listings")
            .HasDefaultValue(0);

        builder.Property(vp => vp.TotalBookings)
            .HasColumnName("total_bookings")
            .HasDefaultValue(0);

        builder.Property(vp => vp.TotalEarnings)
            .HasColumnName("total_earnings")
            .HasPrecision(12, 2)
            .HasDefaultValue(0);

        builder.Property(vp => vp.AverageRating)
            .HasColumnName("average_rating")
            .HasPrecision(3, 2)
            .HasDefaultValue(0);

        builder.Property(vp => vp.TotalReviews)
            .HasColumnName("total_reviews")
            .HasDefaultValue(0);

        builder.Property(vp => vp.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(vp => vp.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(vp => vp.UserId).IsUnique();
        builder.HasIndex(vp => vp.City);
        builder.HasIndex(vp => vp.State);
        builder.HasIndex(vp => vp.BusinessType);
        builder.HasIndex(vp => vp.IsVerified);
        builder.HasIndex(vp => new { vp.Latitude, vp.Longitude });

        // Relationships
        builder.HasOne(vp => vp.User)
            .WithOne(u => u.VendorProfile)
            .HasForeignKey<VendorProfile>(vp => vp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

