using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Search;

public class GlobalSearchResponse
{
    public bool Success { get; set; }
    public GlobalSearchData? Data { get; set; }
    public PaginationInfo? Pagination { get; set; }
}

public class GlobalSearchData
{
    public List<SearchResultDto> Halls { get; set; } = [];
    public List<SearchResultDto> Services { get; set; } = [];
    public List<SearchResultDto> Vendors { get; set; } = [];
}

public class SearchResultDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // hall | service | vendor
}

