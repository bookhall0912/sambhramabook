using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class PhotographyServiceDetailConfiguration : IEntityTypeConfiguration<PhotographyServiceDetail>
{
    public void Configure(EntityTypeBuilder<PhotographyServiceDetail> builder)
    {
        builder.ToTable(nameof(PhotographyServiceDetail), DefaultDatabaseSchema.Name);

        builder.HasKey(p => p.ServiceId)
            .HasName("PK_PhotographyServiceDetails_ServiceId");

        builder.Property(p => p.ServiceId)
            .IsRequired();

        builder.Property(p => p.ExperienceYears)
            .IsRequired();

        builder.Property(p => p.StartingPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.EquipmentJson)
            .HasMaxLength(2000)
            .IsUnicode(false);

        builder.HasOne(p => p.Service)
            .WithOne(s => s.PhotographyServiceDetails)
            .HasForeignKey<PhotographyServiceDetail>(p => p.ServiceId)
            .HasConstraintName($"FK_{nameof(PhotographyServiceDetail)}_{nameof(Service)}")
            .OnDelete(DeleteBehavior.Cascade);
    }
}

