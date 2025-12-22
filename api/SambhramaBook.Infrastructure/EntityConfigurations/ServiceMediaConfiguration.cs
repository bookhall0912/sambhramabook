using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class ServiceMediaConfiguration : IEntityTypeConfiguration<ServiceMedia>
{
    public void Configure(EntityTypeBuilder<ServiceMedia> builder)
    {
        builder.ToTable(nameof(ServiceMedia), DefaultDatabaseSchema.Name);

        builder.HasKey(sm => sm.Id)
            .HasName("PK_ServiceMedia_Id");

        builder.Property(sm => sm.Id)
            .IsRequired();

        builder.Property(sm => sm.ServiceId)
            .IsRequired();

        builder.Property(sm => sm.MediaUrl)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode(false);

        builder.Property(sm => sm.MediaType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(sm => sm.IsCover)
            .IsRequired();

        builder.HasOne(sm => sm.Service)
            .WithMany(s => s.ServiceMedia)
            .HasForeignKey(sm => sm.ServiceId)
            .HasConstraintName($"FK_{nameof(ServiceMedia)}_{nameof(Service)}")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(sm => sm.ServiceId)
            .HasDatabaseName("IX_ServiceMedia_ServiceId");
    }
}

