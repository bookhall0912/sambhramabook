namespace SambhramaBook.Application.Models.VendorDashboard;

public class DashboardOverviewResponse
{
    public bool Success { get; set; }
    public DashboardOverviewData? Data { get; set; }
}

public class DashboardOverviewData
{
    public decimal TotalEarnings { get; set; }
    public int UpcomingEvents { get; set; }
    public int ProfileViews { get; set; }
    public decimal EarningsChange { get; set; }
    public decimal ProfileViewsChange { get; set; }
    public string NextEvent { get; set; } = string.Empty;
    public List<RecentBookingDto> RecentBookings { get; set; } = [];
    public List<ListingSummaryDto> Listings { get; set; } = [];
    public int PendingBookingsCount { get; set; }
}

public class RecentBookingDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class ListingSummaryDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

