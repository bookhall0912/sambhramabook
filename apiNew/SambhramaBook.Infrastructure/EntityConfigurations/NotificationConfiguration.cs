using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");

        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(n => n.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(n => n.NotificationType)
            .HasColumnName("notification_type")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(n => n.Title)
            .HasColumnName("title")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(n => n.Message)
            .HasColumnName("message")
            .IsRequired();

        builder.Property(n => n.Data)
            .HasColumnName("data");

        builder.Property(n => n.IsRead)
            .HasColumnName("is_read")
            .HasDefaultValue(false);

        builder.Property(n => n.ReadAt)
            .HasColumnName("read_at");

        builder.Property(n => n.ActionUrl)
            .HasColumnName("action_url")
            .HasMaxLength(500);

        builder.Property(n => n.Priority)
            .HasColumnName("priority")
            .HasMaxLength(20)
            .HasDefaultValue("NORMAL");

        builder.Property(n => n.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(n => n.UserId);
        builder.HasIndex(n => new { n.UserId, n.IsRead });
        builder.HasIndex(n => n.NotificationType);
        builder.HasIndex(n => n.CreatedAt);

        // Relationships
        builder.HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

