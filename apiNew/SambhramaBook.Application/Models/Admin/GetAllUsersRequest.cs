namespace SambhramaBook.Application.Models.Admin;

public class GetAllUsersRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public string? Status { get; set; } // ACTIVE | INACTIVE | SUSPENDED
}

