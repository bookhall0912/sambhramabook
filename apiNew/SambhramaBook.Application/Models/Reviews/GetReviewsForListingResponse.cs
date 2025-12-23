using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Reviews;

public class GetReviewsForListingResponse
{
    public bool Success { get; set; }
    public List<ReviewDto> Data { get; set; } = [];
    public PaginationInfo? Pagination { get; set; }
}

public class ReviewDto
{
    public string Id { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public string? Comment { get; set; }
    public string Date { get; set; } = string.Empty;
    public bool Verified { get; set; }
}

