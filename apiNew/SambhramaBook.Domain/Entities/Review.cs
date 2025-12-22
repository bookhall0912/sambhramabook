namespace SambhramaBook.Domain.Entities;

public class Review
{
    public long Id { get; set; }
    public long BookingId { get; set; }
    public long ListingId { get; set; }
    public long CustomerId { get; set; }
    public long VendorId { get; set; }

    public required int Rating { get; set; } // 1-5
    public string? Title { get; set; }
    public string? Comment { get; set; }

    // Detailed Ratings
    public int? CleanlinessRating { get; set; } // 1-5
    public int? ServiceRating { get; set; } // 1-5
    public int? ValueRating { get; set; } // 1-5
    public int? LocationRating { get; set; } // 1-5

    public bool IsVerifiedBooking { get; set; } = true;
    public bool IsPublished { get; set; } = true;
    public int IsHelpfulCount { get; set; } = 0;

    public string? VendorResponse { get; set; }
    public DateTime? VendorRespondedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Booking Booking { get; set; } = null!;
    public Listing Listing { get; set; } = null!;
    public User Customer { get; set; } = null!;
    public VendorProfile Vendor { get; set; } = null!;
    public ICollection<ReviewHelpful> HelpfulVotes { get; set; } = [];
}

