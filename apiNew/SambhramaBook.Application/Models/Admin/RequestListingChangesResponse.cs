using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class RequestListingChangesResponse
{
    public bool Success { get; set; }
    public RequestListingChangesResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class RequestListingChangesResponseData
{
    public string Id { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

