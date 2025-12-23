namespace SambhramaBook.Application.Models.VendorBookings;

public class GetVendorBookingsRequest
{
    public string Status { get; set; } = "all"; // all | pending | confirmed
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

