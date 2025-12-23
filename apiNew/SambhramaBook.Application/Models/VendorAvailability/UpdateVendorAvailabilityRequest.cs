namespace SambhramaBook.Application.Models.VendorAvailability;

public class UpdateVendorAvailabilityRequest
{
    public List<DateAvailabilityUpdate> Dates { get; set; } = [];
}

public class DateAvailabilityUpdate
{
    public string Date { get; set; } = string.Empty; // YYYY-MM-DD
    public string Status { get; set; } = string.Empty; // available | booked | blocked
}

