using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain.Entities;

public class VendorProfile
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public required string BusinessName { get; set; }
    public BusinessType BusinessType { get; set; }
    public required string BusinessEmail { get; set; }
    public required string BusinessPhone { get; set; }
    public string? BusinessLogoUrl { get; set; }
    public string? BusinessDescription { get; set; }

    // Address
    public required string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string Pincode { get; set; }
    public string Country { get; set; } = "India";
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    // Business Details
    public string? GstNumber { get; set; }
    public string? PanNumber { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? IfscCode { get; set; }
    public string? BankName { get; set; }
    public string? AccountHolderName { get; set; }

    // Status
    public bool ProfileComplete { get; set; } = false;
    public bool IsVerified { get; set; } = false;
    public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Pending;
    public string? VerificationNotes { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public long? VerifiedBy { get; set; }

    // Statistics
    public int TotalListings { get; set; } = 0;
    public int TotalBookings { get; set; } = 0;
    public decimal TotalEarnings { get; set; } = 0;
    public decimal AverageRating { get; set; } = 0;
    public int TotalReviews { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<Listing> Listings { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Payout> Payouts { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
}

