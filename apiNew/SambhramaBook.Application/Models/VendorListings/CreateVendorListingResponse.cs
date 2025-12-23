using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.VendorListings;

public class CreateVendorListingResponse
{
    public bool Success { get; set; }
    public CreateVendorListingResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class CreateVendorListingResponseData
{
    public string Id { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

