namespace SambhramaBook.Application.Models.VendorEarnings;

public class GetEarningsSummaryRequest
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}

