using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class ServiceAvailabilityConfiguration : IEntityTypeConfiguration<ServiceAvailability>
{
    public void Configure(EntityTypeBuilder<ServiceAvailability> builder)
    {
        builder.ToTable(nameof(ServiceAvailability), DefaultDatabaseSchema.Name);

        builder.HasKey(sa => sa.Id)
            .HasName("PK_ServiceAvailability_Id");

        builder.Property(sa => sa.Id)
            .IsRequired();

        builder.Property(sa => sa.ServiceId)
            .IsRequired();

        builder.Property(sa => sa.Date)
            .IsRequired();

        builder.Property(sa => sa.IsAvailable)
            .IsRequired();

        builder.HasOne(sa => sa.Service)
            .WithMany(s => s.ServiceAvailabilities)
            .HasForeignKey(sa => sa.ServiceId)
            .HasConstraintName($"FK_{nameof(ServiceAvailability)}_{nameof(Service)}")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(sa => new { sa.ServiceId, sa.Date })
            .IsUnique()
            .HasDatabaseName("IX_ServiceAvailability_ServiceId_Date");

        builder.HasIndex(sa => new { sa.ServiceId, sa.IsAvailable, sa.Date })
            .HasDatabaseName("IX_ServiceAvailability_ServiceId_IsAvailable_Date");
    }
}

