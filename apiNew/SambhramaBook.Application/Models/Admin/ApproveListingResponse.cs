using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class ApproveListingResponse
{
    public bool Success { get; set; }
    public ApproveListingResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class ApproveListingResponseData
{
    public string Id { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

