using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.VendorListings;

public class UpdateListingStatusResponse
{
    public bool Success { get; set; }
    public UpdateListingStatusResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class UpdateListingStatusResponseData
{
    public string Id { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

