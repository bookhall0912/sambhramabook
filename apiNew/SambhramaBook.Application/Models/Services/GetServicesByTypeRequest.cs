namespace SambhramaBook.Application.Models.Services;

public class GetServicesByTypeRequest
{
    public string Type { get; set; } = string.Empty; // photography, catering, etc.
    public string? Location { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}

