namespace SambhramaBook.Application.Models.Payment;

public class ProcessPaymentRequest
{
    public string PaymentMethod { get; set; } = string.Empty; // "razorpay" | "stripe" | "cash"
    public decimal Amount { get; set; }
}

