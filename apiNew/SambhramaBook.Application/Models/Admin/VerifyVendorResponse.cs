using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class VerifyVendorResponse
{
    public bool Success { get; set; }
    public VerifyVendorResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class VerifyVendorResponseData
{
    public string Id { get; set; } = string.Empty;
    public string VerificationStatus { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

