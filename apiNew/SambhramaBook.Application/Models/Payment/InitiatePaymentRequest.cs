namespace SambhramaBook.Application.Models.Payment;

public class InitiatePaymentRequest
{
    public string BookingId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty; // "razorpay" | "stripe" | "cash"
}

