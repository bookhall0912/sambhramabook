using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User), DefaultDatabaseSchema.Name);

        builder.HasKey(u => u.Id)
            .HasName("PK_User_Id");

        builder.Property(u => u.Id)
            .IsRequired();

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode(false);

        builder.Property(u => u.Phone)
            .IsRequired()
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(u => u.Email)
            .HasMaxLength(254)
            .IsUnicode(false);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(u => u.IsActive)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.HasIndex(u => u.Phone)
            .HasDatabaseName("IX_User_Phone");

        builder.HasIndex(u => u.Email)
            .HasDatabaseName("IX_User_Email")
            .IsUnique()
            .HasFilter("[Email] IS NOT NULL");
    }
}

