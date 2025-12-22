using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class ReviewHelpfulConfiguration : IEntityTypeConfiguration<ReviewHelpful>
{
    public void Configure(EntityTypeBuilder<ReviewHelpful> builder)
    {
        builder.ToTable("review_helpfuls");

        builder.HasKey(rh => rh.Id);
        builder.Property(rh => rh.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(rh => rh.ReviewId)
            .HasColumnName("review_id")
            .IsRequired();

        builder.Property(rh => rh.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(rh => rh.IsHelpful)
            .HasColumnName("is_helpful")
            .IsRequired();

        builder.Property(rh => rh.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Unique constraint - one vote per user per review
        builder.HasIndex(rh => new { rh.ReviewId, rh.UserId }).IsUnique();

        // Indexes
        builder.HasIndex(rh => rh.ReviewId);
        builder.HasIndex(rh => rh.UserId);

        // Relationships
        builder.HasOne(rh => rh.Review)
            .WithMany(r => r.HelpfulVotes)
            .HasForeignKey(rh => rh.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rh => rh.User)
            .WithMany()
            .HasForeignKey(rh => rh.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

