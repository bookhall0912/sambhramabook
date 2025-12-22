using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain;

public sealed class Vendor
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string BusinessName { get; set; }
    public VerificationStatus VerificationStatus { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Service> Services { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Conversation> Conversations { get; set; } = [];
    public ICollection<Payout> Payouts { get; set; } = [];
}

