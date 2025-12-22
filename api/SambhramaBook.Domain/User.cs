using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain;

public sealed class User
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public string? Email { get; set; }
    public Role Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public Vendor? Vendor { get; set; }
    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Conversation> Conversations { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
}

