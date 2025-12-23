namespace SambhramaBook.Application.Models.Reviews;

public class CreateReviewRequest
{
    public long ListingId { get; set; }
    public decimal Rating { get; set; }
    public string? Comment { get; set; }
    public bool Verified { get; set; }
}

