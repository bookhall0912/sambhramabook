using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Payment;

public class VerifyPaymentResponse
{
    public bool Success { get; set; }
    public VerifyPaymentResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class VerifyPaymentResponseData
{
    public string PaymentId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string BookingId { get; set; } = string.Empty;
}

