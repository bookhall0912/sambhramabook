using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.VendorListings;

public class UploadListingImagesResponse
{
    public bool Success { get; set; }
    public UploadListingImagesResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class UploadListingImagesResponseData
{
    public List<string> Images { get; set; } = [];
}

