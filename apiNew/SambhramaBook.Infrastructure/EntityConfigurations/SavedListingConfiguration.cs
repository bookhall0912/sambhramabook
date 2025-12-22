using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class SavedListingConfiguration : IEntityTypeConfiguration<SavedListing>
{
    public void Configure(EntityTypeBuilder<SavedListing> builder)
    {
        builder.ToTable("saved_listings");

        builder.HasKey(sl => sl.Id);
        builder.Property(sl => sl.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(sl => sl.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(sl => sl.ListingId)
            .HasColumnName("listing_id")
            .IsRequired();

        builder.Property(sl => sl.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Unique constraint
        builder.HasIndex(sl => new { sl.UserId, sl.ListingId }).IsUnique();

        // Indexes
        builder.HasIndex(sl => sl.UserId);
        builder.HasIndex(sl => sl.ListingId);

        // Relationships
        builder.HasOne(sl => sl.User)
            .WithMany(u => u.SavedListings)
            .HasForeignKey(sl => sl.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sl => sl.Listing)
            .WithMany(l => l.SavedListings)
            .HasForeignKey(sl => sl.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

