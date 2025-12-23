namespace SambhramaBook.Application.Models.Admin;

public class GetAllBookingsRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Status { get; set; } // PENDING | CONFIRMED | CANCELLED
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}

