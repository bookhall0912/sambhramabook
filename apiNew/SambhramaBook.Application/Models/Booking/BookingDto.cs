using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Models.Booking;

public class BookingDto
{
    public long Id { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public long ListingId { get; set; }
    public string ListingName { get; set; } = string.Empty;
    public string? ListingImageUrl { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int Days { get; set; }
    public int GuestCount { get; set; }
    public decimal TotalAmount { get; set; }
    public BookingStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string? EventType { get; set; }
    public DateTime CreatedAt { get; set; }
}

