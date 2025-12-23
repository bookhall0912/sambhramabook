namespace SambhramaBook.Application.Models.Admin;

public class GetUserReportResponse
{
    public bool Success { get; set; }
    public UserReportData? Data { get; set; }
}

public class UserReportData
{
    public PeriodDto Period { get; set; } = new();
    public int TotalUsers { get; set; }
    public int NewUsers { get; set; }
    public int ActiveUsers { get; set; }
    public List<UserReportBreakdownDto> Breakdown { get; set; } = [];
}

public class UserReportBreakdownDto
{
    public string Month { get; set; } = string.Empty;
    public int New { get; set; }
    public int Active { get; set; }
}

