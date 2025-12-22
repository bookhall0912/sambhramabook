using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable(nameof(Review), DefaultDatabaseSchema.Name);

        builder.HasKey(r => r.Id)
            .HasName("PK_Review_Id");

        builder.Property(r => r.Id)
            .IsRequired();

        builder.Property(r => r.BookingId)
            .IsRequired();

        builder.Property(r => r.ServiceId)
            .IsRequired();

        builder.Property(r => r.UserId)
            .IsRequired();

        builder.Property(r => r.Rating)
            .IsRequired();

        builder.Property(r => r.Comment)
            .HasMaxLength(2000)
            .IsUnicode(false);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.HasOne(r => r.Booking)
            .WithOne(b => b.Review)
            .HasForeignKey<Review>(r => r.BookingId)
            .HasConstraintName($"FK_{nameof(Review)}_{nameof(Booking)}")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(r => r.Service)
            .WithMany(s => s.Reviews)
            .HasForeignKey(r => r.ServiceId)
            .HasConstraintName($"FK_{nameof(Review)}_{nameof(Service)}")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .HasConstraintName($"FK_{nameof(Review)}_{nameof(User)}")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(r => r.BookingId)
            .IsUnique()
            .HasDatabaseName("IX_Review_BookingId");

        builder.HasIndex(r => r.ServiceId)
            .HasDatabaseName("IX_Review_ServiceId");

        builder.HasIndex(r => new { r.ServiceId, r.Rating })
            .HasDatabaseName("IX_Review_ServiceId_Rating");
    }
}

