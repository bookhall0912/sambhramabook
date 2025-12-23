using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Services;

public class GetServicesByTypeResponse
{
    public bool Success { get; set; }
    public List<ServiceListItemDto> Data { get; set; } = [];
    public PaginationInfo? Pagination { get; set; }
}

public class ServiceListItemDto
{
    public long Id { get; set; }
    public int ServiceType { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string City { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public decimal Rating { get; set; }
    public int ReviewCount { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public List<string> Images { get; set; } = [];
}

