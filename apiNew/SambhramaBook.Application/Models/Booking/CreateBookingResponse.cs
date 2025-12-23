using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Booking;

public class CreateBookingResponse
{
    public bool Success { get; set; }
    public CreateBookingResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class CreateBookingResponseData
{
    public string BookingId { get; set; } = string.Empty;
    public string HallId { get; set; } = string.Empty;
    public string HallName { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int NumberOfDays { get; set; }
    public int Guests { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ConfirmationNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

