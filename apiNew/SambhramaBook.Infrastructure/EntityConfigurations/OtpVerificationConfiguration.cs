using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class OtpVerificationConfiguration : IEntityTypeConfiguration<OtpVerification>
{
    public void Configure(EntityTypeBuilder<OtpVerification> builder)
    {
        builder.ToTable("otp_verifications");

        builder.HasKey(ov => ov.Id);
        builder.Property(ov => ov.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(ov => ov.MobileOrEmail)
            .HasColumnName("mobile_or_email")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(ov => ov.OtpCode)
            .HasColumnName("otp_code")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(ov => ov.OtpType)
            .HasColumnName("otp_type")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ov => ov.IsUsed)
            .HasColumnName("is_used")
            .HasDefaultValue(false);

        builder.Property(ov => ov.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();

        builder.Property(ov => ov.VerifiedAt)
            .HasColumnName("verified_at");

        builder.Property(ov => ov.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(ov => ov.MobileOrEmail);
        builder.HasIndex(ov => new { ov.MobileOrEmail, ov.OtpCode, ov.IsUsed });
        builder.HasIndex(ov => ov.ExpiresAt);
    }
}

