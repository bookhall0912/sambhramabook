namespace SambhramaBook.Application.Models.Admin;

public class GetAllReviewsRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Status { get; set; } // published | pending | rejected
}

