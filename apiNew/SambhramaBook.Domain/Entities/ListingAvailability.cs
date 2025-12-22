using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain.Entities;

public class ListingAvailability
{
    public long Id { get; set; }
    public long ListingId { get; set; }
    public required DateOnly Date { get; set; }
    public AvailabilityStatus Status { get; set; } = AvailabilityStatus.Available;
    public decimal? PriceOverride { get; set; } // Special pricing for specific dates
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Listing Listing { get; set; } = null!;
}

