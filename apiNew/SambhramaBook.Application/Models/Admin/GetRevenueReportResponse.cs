namespace SambhramaBook.Application.Models.Admin;

public class GetRevenueReportResponse
{
    public bool Success { get; set; }
    public RevenueReportData? Data { get; set; }
}

public class RevenueReportData
{
    public PeriodDto Period { get; set; } = new();
    public decimal TotalRevenue { get; set; }
    public decimal TotalCommission { get; set; }
    public List<RevenueReportBreakdownDto> Breakdown { get; set; } = [];
}

public class PeriodDto
{
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
}

public class RevenueReportBreakdownDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal Commission { get; set; }
}

