namespace SambhramaBook.Application.Models.Booking;

public class GetUserBookingsRequest
{
    public string? Status { get; set; } // UPCOMING | PAST | CANCELLED
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

