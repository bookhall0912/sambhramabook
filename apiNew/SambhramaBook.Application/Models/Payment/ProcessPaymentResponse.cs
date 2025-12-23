using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Payment;

public class ProcessPaymentResponse
{
    public bool Success { get; set; }
    public ProcessPaymentResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class ProcessPaymentResponseData
{
    public string PaymentId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentUrl { get; set; } = string.Empty;
}

