namespace SambhramaBook.Application.Models.Admin;

public class GetBookingReportRequest
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Format { get; set; } // json | csv | excel
}

