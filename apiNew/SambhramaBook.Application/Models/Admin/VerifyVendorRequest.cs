namespace SambhramaBook.Application.Models.Admin;

public class VerifyVendorRequest
{
    public string VerificationStatus { get; set; } = string.Empty; // VERIFIED | REJECTED
    public string? Notes { get; set; }
}

