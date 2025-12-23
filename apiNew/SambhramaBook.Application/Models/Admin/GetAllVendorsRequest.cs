namespace SambhramaBook.Application.Models.Admin;

public class GetAllVendorsRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Status { get; set; } // ACTIVE | INACTIVE | SUSPENDED
}

