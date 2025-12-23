namespace SambhramaBook.Application.Models.VendorAvailability;

public class GetVendorAvailabilityResponse
{
    public bool Success { get; set; }
    public VendorAvailabilityData? Data { get; set; }
}

public class VendorAvailabilityData
{
    public string ListingId { get; set; } = string.Empty;
    public string Month { get; set; } = string.Empty;
    public int Year { get; set; }
    public List<AvailabilityDayDto> Days { get; set; } = [];
}

public class AvailabilityDayDto
{
    public int Day { get; set; }
    public string Status { get; set; } = string.Empty; // available | booked | blocked
}

