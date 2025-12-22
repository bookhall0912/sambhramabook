using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure.EntityConfigurations;

public sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable(nameof(Message), DefaultDatabaseSchema.Name);

        builder.HasKey(m => m.Id)
            .HasName("PK_Message_Id");

        builder.Property(m => m.Id)
            .IsRequired();

        builder.Property(m => m.ConversationId)
            .IsRequired();

        builder.Property(m => m.SenderRole)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(m => m.MessageText)
            .IsRequired()
            .HasMaxLength(2000)
            .IsUnicode(false);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.HasOne(m => m.Conversation)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ConversationId)
            .HasConstraintName($"FK_{nameof(Message)}_{nameof(Conversation)}")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => m.ConversationId)
            .HasDatabaseName("IX_Message_ConversationId");

        builder.HasIndex(m => new { m.ConversationId, m.CreatedAt })
            .HasDatabaseName("IX_Message_ConversationId_CreatedAt");
    }
}

