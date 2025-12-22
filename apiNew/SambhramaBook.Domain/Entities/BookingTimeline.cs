namespace SambhramaBook.Domain.Entities;

public class BookingTimeline
{
    public long Id { get; set; }
    public long BookingId { get; set; }
    public string? StatusFrom { get; set; }
    public required string StatusTo { get; set; }
    public long? ChangedBy { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Booking Booking { get; set; } = null!;
}

