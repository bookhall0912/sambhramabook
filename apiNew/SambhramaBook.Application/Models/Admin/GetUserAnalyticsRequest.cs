namespace SambhramaBook.Application.Models.Admin;

public class GetUserAnalyticsRequest
{
    public string Period { get; set; } = "monthly"; // monthly | weekly | daily
}

