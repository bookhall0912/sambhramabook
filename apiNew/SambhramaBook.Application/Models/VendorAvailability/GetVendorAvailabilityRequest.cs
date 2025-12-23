namespace SambhramaBook.Application.Models.VendorAvailability;

public class GetVendorAvailabilityRequest
{
    public long ListingId { get; set; }
    public string Month { get; set; } = string.Empty; // "December" or "12"
    public int Year { get; set; }
}

