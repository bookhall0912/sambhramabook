using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public class ListingAvailabilityConfiguration : IEntityTypeConfiguration<ListingAvailability>
{
    public void Configure(EntityTypeBuilder<ListingAvailability> builder)
    {
        builder.ToTable("listing_availability");

        builder.HasKey(la => la.Id);
        builder.Property(la => la.Id).HasColumnName("id").UseIdentityColumn();

        builder.Property(la => la.ListingId)
            .HasColumnName("listing_id")
            .IsRequired();

        builder.Property(la => la.Date)
            .HasColumnName("date")
            .IsRequired();

        builder.Property(la => la.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .HasDefaultValue(Domain.Enums.AvailabilityStatus.Available);

        builder.Property(la => la.PriceOverride)
            .HasColumnName("price_override")
            .HasPrecision(10, 2);

        builder.Property(la => la.Notes)
            .HasColumnName("notes");

        builder.Property(la => la.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(la => la.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Unique constraint
        builder.HasIndex(la => new { la.ListingId, la.Date }).IsUnique();

        // Indexes
        builder.HasIndex(la => la.ListingId);
        builder.HasIndex(la => la.Date);
        builder.HasIndex(la => la.Status);

        // Relationships
        builder.HasOne(la => la.Listing)
            .WithMany(l => l.Availability)
            .HasForeignKey(la => la.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

