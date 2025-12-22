namespace SambhramaBook.Domain.Entities;

public class ListingImage
{
    public long Id { get; set; }
    public long ListingId { get; set; }
    public required string ImageUrl { get; set; }
    public string ImageType { get; set; } = "GALLERY"; // THUMBNAIL, GALLERY, FLOOR_PLAN, EXTERIOR, INTERIOR
    public int DisplayOrder { get; set; } = 0;
    public string? AltText { get; set; }
    public bool IsPrimary { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Listing Listing { get; set; } = null!;
}

