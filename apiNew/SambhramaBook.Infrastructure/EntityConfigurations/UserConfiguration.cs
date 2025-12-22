using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(u => u.Mobile)
            .HasColumnName("mobile")
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(255);

        builder.Property(u => u.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Role)
            .HasColumnName("role")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(255);

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(u => u.IsEmailVerified)
            .HasColumnName("is_email_verified")
            .HasDefaultValue(false);

        builder.Property(u => u.IsMobileVerified)
            .HasColumnName("is_mobile_verified")
            .HasDefaultValue(false);

        builder.Property(u => u.LastLoginAt)
            .HasColumnName("last_login_at");

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(u => u.DeletedAt)
            .HasColumnName("deleted_at");

        // Indexes
        builder.HasIndex(u => u.Mobile).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Role);
        builder.HasIndex(u => u.IsActive);

        // Relationships
        builder.HasOne(u => u.UserProfile)
            .WithOne(up => up.User)
            .HasForeignKey<UserProfile>(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.VendorProfile)
            .WithOne(vp => vp.User)
            .HasForeignKey<VendorProfile>(vp => vp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

