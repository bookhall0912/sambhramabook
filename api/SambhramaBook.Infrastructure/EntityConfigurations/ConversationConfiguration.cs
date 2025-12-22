using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.ToTable(nameof(Conversation), DefaultDatabaseSchema.Name);

        builder.HasKey(c => c.Id)
            .HasName("PK_Conversation_Id");

        builder.Property(c => c.Id)
            .IsRequired();

        builder.Property(c => c.ServiceId)
            .IsRequired();

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.Property(c => c.VendorId)
            .IsRequired();

        builder.Property(c => c.BookingId);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.HasOne(c => c.Service)
            .WithMany(s => s.Conversations)
            .HasForeignKey(c => c.ServiceId)
            .HasConstraintName($"FK_{nameof(Conversation)}_{nameof(Service)}")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.User)
            .WithMany(u => u.Conversations)
            .HasForeignKey(c => c.UserId)
            .HasConstraintName($"FK_{nameof(Conversation)}_{nameof(User)}")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Vendor)
            .WithMany(v => v.Conversations)
            .HasForeignKey(c => c.VendorId)
            .HasConstraintName($"FK_{nameof(Conversation)}_{nameof(Vendor)}")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Booking)
            .WithOne(b => b.Conversation)
            .HasForeignKey<Conversation>(c => c.BookingId)
            .HasConstraintName($"FK_{nameof(Conversation)}_{nameof(Booking)}")
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);

        builder.HasIndex(c => new { c.ServiceId, c.UserId, c.VendorId })
            .HasDatabaseName("IX_Conversation_ServiceId_UserId_VendorId");

        builder.HasIndex(c => c.BookingId)
            .HasDatabaseName("IX_Conversation_BookingId")
            .HasFilter("[BookingId] IS NOT NULL");
    }
}

