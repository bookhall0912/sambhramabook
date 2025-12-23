namespace SambhramaBook.Application.Models.Booking;

public class RescheduleBookingRequest
{
    public DateOnly NewStartDate { get; set; }
    public DateOnly NewEndDate { get; set; }
    public string? Reason { get; set; }
}

