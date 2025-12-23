using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.VendorListings;

public class DeleteVendorListingResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public ErrorResponse? Error { get; set; }
}

