namespace SambhramaBook.Application.Models.Admin;

public class GetAllPayoutsRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Status { get; set; } // PENDING | PROCESSED | FAILED
}

