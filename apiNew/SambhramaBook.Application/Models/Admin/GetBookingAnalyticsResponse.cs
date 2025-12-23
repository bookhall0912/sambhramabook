namespace SambhramaBook.Application.Models.Admin;

public class GetBookingAnalyticsResponse
{
    public bool Success { get; set; }
    public BookingAnalyticsData? Data { get; set; }
}

public class BookingAnalyticsData
{
    public string Period { get; set; } = string.Empty;
    public List<BookingAnalyticsBreakdownDto> Data { get; set; } = [];
}

public class BookingAnalyticsBreakdownDto
{
    public string Month { get; set; } = string.Empty;
    public int TotalBookings { get; set; }
    public int Confirmed { get; set; }
    public int Cancelled { get; set; }
}

