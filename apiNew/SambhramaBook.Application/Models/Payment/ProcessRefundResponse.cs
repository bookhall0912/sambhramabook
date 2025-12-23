using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Payment;

public class ProcessRefundResponse
{
    public bool Success { get; set; }
    public ProcessRefundResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class ProcessRefundResponseData
{
    public string RefundId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
}

