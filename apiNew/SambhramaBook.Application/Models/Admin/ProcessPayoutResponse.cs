using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class ProcessPayoutResponse
{
    public bool Success { get; set; }
    public ProcessPayoutResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class ProcessPayoutResponseData
{
    public string Id { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

