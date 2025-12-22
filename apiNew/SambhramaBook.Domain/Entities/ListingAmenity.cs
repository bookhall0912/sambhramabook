namespace SambhramaBook.Domain.Entities;

public class ListingAmenity
{
    public long Id { get; set; }
    public long ListingId { get; set; }
    public required string AmenityName { get; set; }
    public string? AmenityCategory { get; set; } // BASIC, PREMIUM, FOOD, TECHNOLOGY
    public string? IconUrl { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Listing Listing { get; set; } = null!;
}

