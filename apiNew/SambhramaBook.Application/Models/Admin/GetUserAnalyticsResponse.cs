namespace SambhramaBook.Application.Models.Admin;

public class GetUserAnalyticsResponse
{
    public bool Success { get; set; }
    public UserAnalyticsData? Data { get; set; }
}

public class UserAnalyticsData
{
    public string Period { get; set; } = string.Empty;
    public List<UserAnalyticsBreakdownDto> Data { get; set; } = [];
}

public class UserAnalyticsBreakdownDto
{
    public string Month { get; set; } = string.Empty;
    public int NewUsers { get; set; }
    public int ActiveUsers { get; set; }
}

