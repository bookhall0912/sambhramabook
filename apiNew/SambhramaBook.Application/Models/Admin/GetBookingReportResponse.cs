namespace SambhramaBook.Application.Models.Admin;

public class GetBookingReportResponse
{
    public bool Success { get; set; }
    public BookingReportData? Data { get; set; }
}

public class BookingReportData
{
    public PeriodDto Period { get; set; } = new();
    public int TotalBookings { get; set; }
    public int Confirmed { get; set; }
    public int Cancelled { get; set; }
    public List<BookingReportBreakdownDto> Breakdown { get; set; } = [];
}

public class BookingReportBreakdownDto
{
    public string Month { get; set; } = string.Empty;
    public int Total { get; set; }
    public int Confirmed { get; set; }
    public int Cancelled { get; set; }
}

