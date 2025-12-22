using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("user_profiles");

        builder.HasKey(up => up.Id);
        builder.Property(up => up.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(up => up.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(up => up.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(255);

        builder.Property(up => up.DateOfBirth)
            .HasColumnName("date_of_birth");

        builder.Property(up => up.Gender)
            .HasColumnName("gender")
            .HasMaxLength(20);

        builder.Property(up => up.ProfileImageUrl)
            .HasColumnName("profile_image_url");

        builder.Property(up => up.AddressLine1)
            .HasColumnName("address_line1")
            .HasMaxLength(255);

        builder.Property(up => up.AddressLine2)
            .HasColumnName("address_line2")
            .HasMaxLength(255);

        builder.Property(up => up.City)
            .HasColumnName("city")
            .HasMaxLength(100);

        builder.Property(up => up.State)
            .HasColumnName("state")
            .HasMaxLength(100);

        builder.Property(up => up.Pincode)
            .HasColumnName("pincode")
            .HasMaxLength(10);

        builder.Property(up => up.Country)
            .HasColumnName("country")
            .HasMaxLength(100)
            .HasDefaultValue("India");

        builder.Property(up => up.AlternatePhone)
            .HasColumnName("alternate_phone")
            .HasMaxLength(15);

        builder.Property(up => up.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(up => up.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(up => up.UserId).IsUnique();

        // Relationships
        builder.HasOne(up => up.User)
            .WithOne(u => u.UserProfile)
            .HasForeignKey<UserProfile>(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

