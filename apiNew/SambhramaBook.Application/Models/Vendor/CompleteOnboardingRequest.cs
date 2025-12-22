namespace SambhramaBook.Application.Models.Vendor;

public class CompleteOnboardingRequest
{
    public required string BusinessName { get; set; }
    public required string BusinessType { get; set; } // "Hall Owner" | "Service Provider"
    public required string BusinessEmail { get; set; }
    public required string BusinessPhone { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string Pincode { get; set; }
    public string? GstNumber { get; set; }
    public string? PanNumber { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? IfscCode { get; set; }
}

