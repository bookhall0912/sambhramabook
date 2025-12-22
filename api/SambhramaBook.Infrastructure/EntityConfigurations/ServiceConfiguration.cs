using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable(nameof(Service), DefaultDatabaseSchema.Name);

        builder.HasKey(s => s.Id)
            .HasName("PK_Service_Id");

        builder.Property(s => s.Id)
            .IsRequired();

        builder.Property(s => s.VendorId)
            .IsRequired();

        builder.Property(s => s.ServiceType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode(false);

        builder.Property(s => s.Description)
            .HasMaxLength(2000)
            .IsUnicode(false);

        builder.Property(s => s.City)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(s => s.Latitude)
            .IsRequired();

        builder.Property(s => s.Longitude)
            .IsRequired();

        builder.Property(s => s.SearchPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(s => s.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.HasOne(s => s.Vendor)
            .WithMany(v => v.Services)
            .HasForeignKey(s => s.VendorId)
            .HasConstraintName($"FK_{nameof(Service)}_{nameof(Vendor)}")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(s => s.VendorId)
            .HasDatabaseName("IX_Service_VendorId");

        builder.HasIndex(s => new { s.ServiceType, s.Status })
            .HasDatabaseName("IX_Service_ServiceType_Status");

        builder.HasIndex(s => s.City)
            .HasDatabaseName("IX_Service_City");
    }
}

