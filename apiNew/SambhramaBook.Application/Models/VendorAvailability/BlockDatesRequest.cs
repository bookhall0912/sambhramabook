namespace SambhramaBook.Application.Models.VendorAvailability;

public class BlockDatesRequest
{
    public List<string> Dates { get; set; } = []; // YYYY-MM-DD format
    public string? Reason { get; set; }
}

