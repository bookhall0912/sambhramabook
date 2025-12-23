namespace SambhramaBook.Application.Models.VendorDashboard;

public class GetRecentBookingsResponse
{
    public bool Success { get; set; }
    public List<RecentBookingDto> Data { get; set; } = [];
}

