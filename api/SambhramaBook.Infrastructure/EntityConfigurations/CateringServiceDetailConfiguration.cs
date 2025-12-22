using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class CateringServiceDetailConfiguration : IEntityTypeConfiguration<CateringServiceDetail>
{
    public void Configure(EntityTypeBuilder<CateringServiceDetail> builder)
    {
        builder.ToTable(nameof(CateringServiceDetail), DefaultDatabaseSchema.Name);

        builder.HasKey(c => c.ServiceId)
            .HasName("PK_CateringServiceDetails_ServiceId");

        builder.Property(c => c.ServiceId)
            .IsRequired();

        builder.Property(c => c.CuisineTypesJson)
            .HasMaxLength(2000)
            .IsUnicode(false);

        builder.Property(c => c.VegNonVegType)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(c => c.MinimumOrderValue)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne(c => c.Service)
            .WithOne(s => s.CateringServiceDetails)
            .HasForeignKey<CateringServiceDetail>(c => c.ServiceId)
            .HasConstraintName($"FK_{nameof(CateringServiceDetail)}_{nameof(Service)}")
            .OnDelete(DeleteBehavior.Cascade);
    }
}

