using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.VendorBookings;

public class GetVendorBookingsResponse
{
    public bool Success { get; set; }
    public List<VendorBookingDto> Data { get; set; } = [];
    public PaginationInfo? Pagination { get; set; }
}

public class VendorBookingDto
{
    public long Id { get; set; }
    public string BookingId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string VenueName { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public int Days { get; set; }
    public int Guests { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty; // CONFIRMED | PENDING | CANCELLED
}

