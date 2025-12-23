namespace SambhramaBook.Application.Models.Admin;

public class UpdateBookingStatusRequest
{
    public string Status { get; set; } = string.Empty; // PENDING | CONFIRMED | CANCELLED
}

