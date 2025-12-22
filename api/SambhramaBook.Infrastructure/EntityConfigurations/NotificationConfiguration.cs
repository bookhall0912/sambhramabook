using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable(nameof(Notification), DefaultDatabaseSchema.Name);

        builder.HasKey(n => n.Id)
            .HasName("PK_Notification_Id");

        builder.Property(n => n.Id)
            .IsRequired();

        builder.Property(n => n.UserId)
            .IsRequired();

        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode(false);

        builder.Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(1000)
            .IsUnicode(false);

        builder.Property(n => n.IsRead)
            .IsRequired();

        builder.Property(n => n.CreatedAt)
            .IsRequired();

        builder.HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .HasConstraintName($"FK_{nameof(Notification)}_{nameof(User)}")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(n => n.UserId)
            .HasDatabaseName("IX_Notification_UserId");

        builder.HasIndex(n => new { n.UserId, n.IsRead, n.CreatedAt })
            .HasDatabaseName("IX_Notification_UserId_IsRead_CreatedAt");
    }
}

