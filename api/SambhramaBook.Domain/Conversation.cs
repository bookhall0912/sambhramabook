using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain;

public sealed class Conversation
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public Guid UserId { get; set; }
    public Guid VendorId { get; set; }
    public Guid? BookingId { get; set; }
    public ConversationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public Service Service { get; set; } = null!;
    public User User { get; set; } = null!;
    public Vendor Vendor { get; set; } = null!;
    public Booking? Booking { get; set; }
    public ICollection<Message> Messages { get; set; } = [];
}

