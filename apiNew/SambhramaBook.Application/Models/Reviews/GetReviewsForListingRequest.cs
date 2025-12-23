namespace SambhramaBook.Application.Models.Reviews;

public class GetReviewsForListingRequest
{
    public long ListingId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

