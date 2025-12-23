using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class RejectListingResponse
{
    public bool Success { get; set; }
    public RejectListingResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class RejectListingResponseData
{
    public string Id { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

