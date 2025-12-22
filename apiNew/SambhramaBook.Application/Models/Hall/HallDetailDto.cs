namespace SambhramaBook.Application.Models.Hall;

public class HallDetailDto : HallListItemDto
{
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public List<string> Images { get; set; } = [];
    public List<AmenityDto> FullAmenities { get; set; } = [];
    public List<ReviewDto> Reviews { get; set; } = [];
    public VendorInfoDto Vendor { get; set; } = new();
    public string? CancellationPolicy { get; set; }
    public int? AreaSqft { get; set; }
    public int? ParkingCapacity { get; set; }
}

public class AmenityDto
{
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? IconUrl { get; set; }
}

public class ReviewDto
{
    public long Id { get; set; }
    public string Author { get; set; } = string.Empty; // Alias for UserName to match API spec
    public string UserName { get; set; } = string.Empty; // Keep for backward compatibility
    public decimal Rating { get; set; } // Changed to decimal to match spec
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public string Date { get; set; } = string.Empty; // ISO date string (YYYY-MM-DD)
    public DateTime CreatedAt { get; set; } // Keep for internal use
    public bool Verified { get; set; } = true; // Verified booking flag
}

public class VendorInfoDto
{
    public long Id { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string? BusinessLogoUrl { get; set; }
    public decimal AverageRating { get; set; }
    public int TotalReviews { get; set; }
}

