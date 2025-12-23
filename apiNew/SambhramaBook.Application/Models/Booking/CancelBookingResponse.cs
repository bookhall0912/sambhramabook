using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Booking;

public class CancelBookingResponse
{
    public bool Success { get; set; }
    public CancelBookingResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class CancelBookingResponseData
{
    public string BookingId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal RefundAmount { get; set; }
    public string Message { get; set; } = string.Empty;
}

