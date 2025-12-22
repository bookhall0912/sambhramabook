using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain;

public sealed class Booking
{
    public Guid Id { get; set; }
    public required string BookingReference { get; set; }
    public Guid UserId { get; set; }
    public Guid VendorId { get; set; }
    public Guid ServiceId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int Quantity { get; set; }
    public BookingStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
    public Vendor Vendor { get; set; } = null!;
    public Service Service { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = [];
    public Commission? Commission { get; set; }
    public Conversation? Conversation { get; set; }
    public Review? Review { get; set; }
}

