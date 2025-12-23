using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Booking;

public class RescheduleBookingResponse
{
    public bool Success { get; set; }
    public RescheduleBookingResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class RescheduleBookingResponseData
{
    public string BookingId { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Message { get; set; } = string.Empty;
}

