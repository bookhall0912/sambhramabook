using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Reviews;

public class AddVendorResponseResponse
{
    public bool Success { get; set; }
    public AddVendorResponseResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class AddVendorResponseResponseData
{
    public string Id { get; set; } = string.Empty;
    public string? VendorResponse { get; set; }
    public string Message { get; set; } = string.Empty;
}

