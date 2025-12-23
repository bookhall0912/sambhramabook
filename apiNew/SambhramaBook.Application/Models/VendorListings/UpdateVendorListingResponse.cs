using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.VendorListings;

public class UpdateVendorListingResponse
{
    public bool Success { get; set; }
    public UpdateVendorListingResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class UpdateVendorListingResponseData
{
    public string Id { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

