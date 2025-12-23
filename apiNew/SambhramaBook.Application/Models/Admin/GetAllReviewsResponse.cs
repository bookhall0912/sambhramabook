using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class GetAllReviewsResponse
{
    public bool Success { get; set; }
    public List<AdminReviewDto> Data { get; set; } = [];
    public PaginationInfo? Pagination { get; set; }
}

public class AdminReviewDto
{
    public string Id { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ListingId { get; set; } = string.Empty;
    public string ListingName { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public string? Comment { get; set; }
    public string Date { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool Verified { get; set; }
}

