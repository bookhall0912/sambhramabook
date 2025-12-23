namespace SambhramaBook.Application.Models.Admin;

public class UpdateVendorStatusRequest
{
    public string Status { get; set; } = string.Empty; // ACTIVE | INACTIVE | SUSPENDED
}

