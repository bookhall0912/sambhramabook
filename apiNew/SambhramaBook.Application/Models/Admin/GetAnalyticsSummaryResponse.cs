namespace SambhramaBook.Application.Models.Admin;

public class GetAnalyticsSummaryResponse
{
    public bool Success { get; set; }
    public AnalyticsSummaryData? Data { get; set; }
}

public class AnalyticsSummaryData
{
    public decimal TotalRevenue { get; set; }
    public int TotalBookings { get; set; }
    public int TotalUsers { get; set; }
    public int TotalVendors { get; set; }
    public decimal AverageBookingValue { get; set; }
}

