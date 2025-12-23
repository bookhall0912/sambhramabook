namespace SambhramaBook.Application.Models.VendorBookings;

public class CreateVendorBookingRequest
{
    public long ListingId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string? CustomerEmail { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int Guests { get; set; }
    public decimal Amount { get; set; }
}

