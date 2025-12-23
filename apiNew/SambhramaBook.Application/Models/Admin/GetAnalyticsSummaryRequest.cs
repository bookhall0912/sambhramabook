namespace SambhramaBook.Application.Models.Admin;

public class GetAnalyticsSummaryRequest
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}

