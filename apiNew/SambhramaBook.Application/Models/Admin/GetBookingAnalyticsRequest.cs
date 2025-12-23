namespace SambhramaBook.Application.Models.Admin;

public class GetBookingAnalyticsRequest
{
    public string Period { get; set; } = "monthly"; // monthly | weekly | daily
}

