using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class GetAllBookingsResponse
{
    public bool Success { get; set; }
    public List<AdminBookingDto> Data { get; set; } = [];
    public PaginationInfo? Pagination { get; set; }
}

public class AdminBookingDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string VenueName { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
}

