using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class ServiceAmenityConfiguration : IEntityTypeConfiguration<ServiceAmenity>
{
    public void Configure(EntityTypeBuilder<ServiceAmenity> builder)
    {
        builder.ToTable(nameof(ServiceAmenity), DefaultDatabaseSchema.Name);

        builder.HasKey(sa => new { sa.ServiceId, sa.AmenityId })
            .HasName("PK_ServiceAmenity_ServiceId_AmenityId");

        builder.Property(sa => sa.ServiceId)
            .IsRequired();

        builder.Property(sa => sa.AmenityId)
            .IsRequired();

        builder.HasOne(sa => sa.Service)
            .WithMany(s => s.ServiceAmenities)
            .HasForeignKey(sa => sa.ServiceId)
            .HasConstraintName($"FK_{nameof(ServiceAmenity)}_{nameof(Service)}")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sa => sa.Amenity)
            .WithMany(a => a.ServiceAmenities)
            .HasForeignKey(sa => sa.AmenityId)
            .HasConstraintName($"FK_{nameof(ServiceAmenity)}_{nameof(Amenity)}")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(sa => sa.ServiceId)
            .HasDatabaseName("IX_ServiceAmenity_ServiceId");

        builder.HasIndex(sa => sa.AmenityId)
            .HasDatabaseName("IX_ServiceAmenity_AmenityId");
    }
}

