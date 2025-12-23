namespace SambhramaBook.Application.Models.Payment;

public class VerifyPaymentRequest
{
    public string PaymentId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
}

