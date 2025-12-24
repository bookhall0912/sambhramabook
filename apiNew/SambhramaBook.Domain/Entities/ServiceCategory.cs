namespace SambhramaBook.Domain.Entities;

public class ServiceCategory
{
    public long Id { get; set; }
    public required string Code { get; set; } // photography, catering, etc.
    public required string DisplayName { get; set; }
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public string? BackgroundImageUrl { get; set; }
    public string? ThemeColor { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Listing> Listings { get; set; } = [];
}

