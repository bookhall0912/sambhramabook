namespace SambhramaBook.Application.Models.Admin;

public class GetUserReportRequest
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Format { get; set; } // json | csv | excel
}

