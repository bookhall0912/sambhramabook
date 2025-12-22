using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class EventManagementServiceDetailConfiguration : IEntityTypeConfiguration<EventManagementServiceDetail>
{
    public void Configure(EntityTypeBuilder<EventManagementServiceDetail> builder)
    {
        builder.ToTable(nameof(EventManagementServiceDetail), DefaultDatabaseSchema.Name);

        builder.HasKey(e => e.ServiceId)
            .HasName("PK_EventManagementServiceDetails_ServiceId");

        builder.Property(e => e.ServiceId)
            .IsRequired();

        builder.Property(e => e.EventTypesJson)
            .HasMaxLength(2000)
            .IsUnicode(false);

        builder.Property(e => e.TeamSize)
            .IsRequired();

        builder.Property(e => e.StartingPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne(e => e.Service)
            .WithOne(s => s.EventManagementServiceDetails)
            .HasForeignKey<EventManagementServiceDetail>(e => e.ServiceId)
            .HasConstraintName($"FK_{nameof(EventManagementServiceDetail)}_{nameof(Service)}")
            .OnDelete(DeleteBehavior.Cascade);
    }
}

