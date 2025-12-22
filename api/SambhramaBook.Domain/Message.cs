using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain;

public sealed class Message
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public SenderRole SenderRole { get; set; }
    public required string MessageText { get; set; }
    public DateTime CreatedAt { get; set; }

    public Conversation Conversation { get; set; } = null!;
}

