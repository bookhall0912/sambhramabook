using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Booking;

public class GetUserBookingsResponse
{
    public bool Success { get; set; }
    public List<UserBookingDto> Data { get; set; } = [];
    public PaginationInfo? Pagination { get; set; }
}

public class UserBookingDto
{
    public long Id { get; set; }
    public string ReferenceId { get; set; } = string.Empty;
    public string VenueName { get; set; } = string.Empty;
    public string? VenueImage { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int Days { get; set; }
    public int GuestCount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string? EventType { get; set; }
}

