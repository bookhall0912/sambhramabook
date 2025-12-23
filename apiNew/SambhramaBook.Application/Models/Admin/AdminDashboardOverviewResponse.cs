namespace SambhramaBook.Application.Models.Admin;

public class AdminDashboardOverviewResponse
{
    public bool Success { get; set; }
    public AdminDashboardOverviewData? Data { get; set; }
}

public class AdminDashboardOverviewData
{
    public int TotalUsers { get; set; }
    public int ActiveVendors { get; set; }
    public int TotalBookings { get; set; }
    public decimal PlatformRevenue { get; set; }
    public decimal UsersChange { get; set; }
    public decimal VendorsChange { get; set; }
    public decimal BookingsChange { get; set; }
    public decimal RevenueChange { get; set; }
    public List<PendingListingDto> PendingListings { get; set; } = [];
    public int PendingCount { get; set; }
    public int PayoutsCount { get; set; }
}

public class PendingListingDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public string? VendorAvatar { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Submitted { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

