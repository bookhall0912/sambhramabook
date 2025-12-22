namespace SambhramaBook.Domain.Entities;

public class BookingGuest
{
    public long Id { get; set; }
    public long BookingId { get; set; }
    public required string GuestName { get; set; }
    public string? GuestEmail { get; set; }
    public string? GuestPhone { get; set; }
    public bool IsPrimaryContact { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Booking Booking { get; set; } = null!;
}

