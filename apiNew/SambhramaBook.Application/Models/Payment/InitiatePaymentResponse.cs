using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Payment;

public class InitiatePaymentResponse
{
    public bool Success { get; set; }
    public InitiatePaymentResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class InitiatePaymentResponseData
{
    public string PaymentId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string PaymentUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

