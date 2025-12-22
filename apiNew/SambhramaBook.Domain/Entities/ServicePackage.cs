namespace SambhramaBook.Domain.Entities;

public class ServicePackage
{
    public long Id { get; set; }
    public long ListingId { get; set; }
    public required string PackageName { get; set; }
    public string? Description { get; set; }
    public required decimal Price { get; set; }
    public int? DurationHours { get; set; }
    public string? Includes { get; set; } // JSON array of included items
    public int DisplayOrder { get; set; } = 0;
    public bool IsPopular { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Listing Listing { get; set; } = null!;
}

