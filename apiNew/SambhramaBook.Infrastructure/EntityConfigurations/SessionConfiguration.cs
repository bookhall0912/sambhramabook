using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("sessions");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(s => s.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(s => s.Token)
            .HasColumnName("token")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(s => s.RefreshToken)
            .HasColumnName("refresh_token")
            .HasMaxLength(500);

        builder.Property(s => s.DeviceInfo)
            .HasColumnName("device_info")
            .HasMaxLength(255);

        builder.Property(s => s.IpAddress)
            .HasColumnName("ip_address")
            .HasMaxLength(50);

        builder.Property(s => s.UserAgent)
            .HasColumnName("user_agent")
            .HasMaxLength(500);

        builder.Property(s => s.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(s => s.LastActivityAt)
            .HasColumnName("last_activity_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(s => s.UserId);
        builder.HasIndex(s => s.Token).IsUnique();
        builder.HasIndex(s => s.RefreshToken);
        builder.HasIndex(s => s.ExpiresAt);

        // Relationships
        builder.HasOne(s => s.User)
            .WithMany(u => u.Sessions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

