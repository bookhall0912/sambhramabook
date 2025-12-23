namespace SambhramaBook.Application.Models.Booking;

public class BookingDetailDto
{
    public long Id { get; set; }
    public string BookingId { get; set; } = string.Empty;
    public string ReferenceId { get; set; } = string.Empty;
    public string VenueName { get; set; } = string.Empty;
    public string? VenueImage { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int Days { get; set; }
    public int GuestCount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty; // UPCOMING, PAST, CANCELLED, PENDING_CONFIRMATION
    public string PaymentStatus { get; set; } = string.Empty; // PAID, PENDING, REFUNDED
    public string? EventType { get; set; }
    public ContactInfoDto? ContactInfo { get; set; }
    public string? SpecialRequests { get; set; }
}

