namespace SambhramaBook.Application.Models.VendorListings;

public class CreateVendorListingRequest
{
    public string Name { get; set; } = string.Empty;
    public string? HallType { get; set; }
    public string? YearEstablished { get; set; }
    public string? Description { get; set; }
    public string? AddressLine1 { get; set; }
    public string? Area { get; set; }
    public string? City { get; set; }
    public string? Pincode { get; set; }
    public int? FloatingCapacity { get; set; }
    public int? DiningCapacity { get; set; }
    public decimal PricePerDay { get; set; }
    public decimal AdvanceAmount { get; set; }
    public string? CancellationPolicy { get; set; }
    public List<string>? Amenities { get; set; }
    public List<string>? Images { get; set; }
    public string Status { get; set; } = "DRAFT"; // DRAFT | ACTIVE
}

