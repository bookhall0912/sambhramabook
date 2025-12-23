namespace SambhramaBook.Application.Models.Admin;

public class GetRevenueAnalyticsRequest
{
    public string Period { get; set; } = "monthly"; // monthly | weekly | daily
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}

