using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.VendorBookings;

public class RejectBookingResponse
{
    public bool Success { get; set; }
    public RejectBookingResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class RejectBookingResponseData
{
    public string BookingId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

