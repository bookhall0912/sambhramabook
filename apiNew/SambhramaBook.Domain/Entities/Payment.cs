using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain.Entities;

public class Payment
{
    public long Id { get; set; }
    public long BookingId { get; set; }
    public required string PaymentReference { get; set; }
    public required decimal Amount { get; set; }
    public string Currency { get; set; } = "INR";
    public required string PaymentMethod { get; set; }
    public string? PaymentGateway { get; set; } // 'RAZORPAY', 'STRIPE', 'PAYU', etc.
    public string? GatewayTransactionId { get; set; }
    public string? GatewayResponse { get; set; } // JSON response from gateway

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string? FailureReason { get; set; }

    public DateTime? PaidAt { get; set; }
    public DateTime? RefundedAt { get; set; }
    public decimal RefundAmount { get; set; } = 0;
    public string? RefundReason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Booking Booking { get; set; } = null!;
}

