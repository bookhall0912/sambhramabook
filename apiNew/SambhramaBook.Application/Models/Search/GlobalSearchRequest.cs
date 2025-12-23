namespace SambhramaBook.Application.Models.Search;

public class GlobalSearchRequest
{
    public string Query { get; set; } = string.Empty;
    public string Type { get; set; } = "all"; // hall | service | vendor | all
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

