using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class HallServiceDetailConfiguration : IEntityTypeConfiguration<HallServiceDetail>
{
    public void Configure(EntityTypeBuilder<HallServiceDetail> builder)
    {
        builder.ToTable(nameof(HallServiceDetail), DefaultDatabaseSchema.Name);

        builder.HasKey(h => h.ServiceId)
            .HasName("PK_HallServiceDetails_ServiceId");

        builder.Property(h => h.ServiceId)
            .IsRequired();

        builder.Property(h => h.Capacity)
            .IsRequired();

        builder.Property(h => h.MinCapacity)
            .IsRequired(false);

        builder.Property(h => h.MaxCapacity)
            .IsRequired(false);

        builder.Property(h => h.Rooms)
            .IsRequired();

        builder.Property(h => h.PricePerDay)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(h => h.AmenitiesJson)
            .HasMaxLength(2000)
            .IsUnicode(false);

        builder.Property(h => h.ParkingAvailable)
            .IsRequired();

        builder.Property(h => h.CancellationPolicy)
            .HasMaxLength(1000)
            .IsUnicode(false);

        builder.HasOne(h => h.Service)
            .WithOne(s => s.HallServiceDetails)
            .HasForeignKey<HallServiceDetail>(h => h.ServiceId)
            .HasConstraintName($"FK_{nameof(HallServiceDetail)}_{nameof(Service)}")
            .OnDelete(DeleteBehavior.Cascade);
    }
}

