using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain.Entities;

public class Booking
{
    public long Id { get; set; }
    public required string BookingReference { get; set; } // e.g., SB-8824-X901
    public long CustomerId { get; set; }
    public long VendorId { get; set; }
    public long ListingId { get; set; }

    // Booking Details
    public string? EventType { get; set; } // e.g., 'Wedding Reception', 'Corporate Event'
    public string? EventName { get; set; }
    public required int GuestCount { get; set; }
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public required int DurationDays { get; set; }

    // Service Package (if service booking)
    public long? ServicePackageId { get; set; }

    // Pricing
    public required decimal BaseAmount { get; set; }
    public decimal DiscountAmount { get; set; } = 0;
    public decimal TaxAmount { get; set; } = 0;
    public decimal PlatformFee { get; set; } = 0;
    public required decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; } = 0;
    public decimal RefundAmount { get; set; } = 0;

    // Status
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    // Payment
    public string? PaymentMethod { get; set; } // 'UPI', 'CARD', 'NETBANKING', 'WALLET'
    public string? PaymentTransactionId { get; set; }
    public DateTime? PaymentDate { get; set; }

    // Cancellation
    public string? CancellationReason { get; set; }
    public decimal CancellationFee { get; set; } = 0;
    public DateTime? CancelledAt { get; set; }
    public long? CancelledBy { get; set; }

    // Special Requirements
    public string? SpecialRequirements { get; set; }
    public string? AdditionalNotes { get; set; }

    // Vendor Response
    public string VendorStatus { get; set; } = "PENDING"; // PENDING, APPROVED, REJECTED
    public string? VendorResponseNotes { get; set; }
    public DateTime? VendorRespondedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Navigation properties
    public User Customer { get; set; } = null!;
    public VendorProfile Vendor { get; set; } = null!;
    public Listing Listing { get; set; } = null!;
    public ServicePackage? ServicePackage { get; set; }
    public ICollection<BookingGuest> Guests { get; set; } = [];
    public ICollection<BookingTimeline> Timeline { get; set; } = [];
    public ICollection<Payment> Payments { get; set; } = [];
    public Review? Review { get; set; }
}

