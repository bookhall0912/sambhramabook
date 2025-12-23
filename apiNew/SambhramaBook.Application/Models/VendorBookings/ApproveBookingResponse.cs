using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.VendorBookings;

public class ApproveBookingResponse
{
    public bool Success { get; set; }
    public ApproveBookingResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class ApproveBookingResponseData
{
    public string BookingId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

