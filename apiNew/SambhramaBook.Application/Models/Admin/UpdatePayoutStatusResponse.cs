using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class UpdatePayoutStatusResponse
{
    public bool Success { get; set; }
    public UpdatePayoutStatusResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class UpdatePayoutStatusResponseData
{
    public string Id { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

