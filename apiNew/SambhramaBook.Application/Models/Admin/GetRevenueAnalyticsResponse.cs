namespace SambhramaBook.Application.Models.Admin;

public class GetRevenueAnalyticsResponse
{
    public bool Success { get; set; }
    public RevenueAnalyticsData? Data { get; set; }
}

public class RevenueAnalyticsData
{
    public string Period { get; set; } = string.Empty;
    public List<RevenueBreakdownDto> Data { get; set; } = [];
}

public class RevenueBreakdownDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public int Bookings { get; set; }
}

