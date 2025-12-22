namespace SambhramaBook.Domain.Entities;

public class SavedListing
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long ListingId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User User { get; set; } = null!;
    public Listing Listing { get; set; } = null!;
}

