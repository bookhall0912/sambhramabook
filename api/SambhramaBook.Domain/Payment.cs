using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain;

public sealed class Payment
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
    public required string PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string? TransactionReference { get; set; }
    public DateTime CreatedAt { get; set; }

    public Booking Booking { get; set; } = null!;
}

