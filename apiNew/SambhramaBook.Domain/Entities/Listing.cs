using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain.Entities;

public class Listing
{
    public long Id { get; set; }
    public long VendorId { get; set; }
    public ListingType ListingType { get; set; }
    public required string Title { get; set; }
    public required string Slug { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }

    // Location
    public required string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string Pincode { get; set; }
    public string Country { get; set; } = "India";
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    // For Halls
    public int? CapacityMin { get; set; }
    public int? CapacityMax { get; set; }
    public int? AreaSqft { get; set; }
    public int? ParkingCapacity { get; set; }

    // Pricing
    public required decimal BasePrice { get; set; }
    public decimal? PricePerHour { get; set; }
    public decimal? PricePerDay { get; set; }
    public string Currency { get; set; } = "INR";
    public string? CancellationPolicy { get; set; }

    // Status
    public ListingStatus Status { get; set; } = ListingStatus.Draft;
    public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;
    public string? ApprovalNotes { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public long? ApprovedBy { get; set; }

    // Statistics
    public int ViewCount { get; set; } = 0;
    public int BookingCount { get; set; } = 0;
    public decimal AverageRating { get; set; } = 0;
    public int TotalReviews { get; set; } = 0;

    // SEO
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PublishedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation properties
    public VendorProfile Vendor { get; set; } = null!;
    public ICollection<ListingImage> Images { get; set; } = [];
    public ICollection<ListingAmenity> Amenities { get; set; } = [];
    public ICollection<ServicePackage> ServicePackages { get; set; } = [];
    public ICollection<ListingAvailability> Availability { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<SavedListing> SavedListings { get; set; } = [];
}

