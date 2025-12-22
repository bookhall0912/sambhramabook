namespace SambhramaBook.Domain.Entities;

public class Payout
{
    public long Id { get; set; }
    public long VendorId { get; set; }
    public long? BookingId { get; set; }
    public required decimal Amount { get; set; }
    public required decimal PlatformCommission { get; set; }
    public required decimal NetAmount { get; set; }

    public string Status { get; set; } = "PENDING"; // PENDING, PROCESSING, COMPLETED, FAILED, CANCELLED
    public string? PayoutMethod { get; set; } // 'BANK_TRANSFER', 'UPI', 'CHEQUE'
    public string? TransactionReference { get; set; }
    public long? BankAccountId { get; set; } // Reference to vendor's bank account

    public DateTime? ProcessedAt { get; set; }
    public long? ProcessedBy { get; set; }
    public string? FailureReason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public VendorProfile Vendor { get; set; } = null!;
    public Booking? Booking { get; set; }
}

