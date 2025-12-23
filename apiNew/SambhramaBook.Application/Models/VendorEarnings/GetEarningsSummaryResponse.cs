namespace SambhramaBook.Application.Models.VendorEarnings;

public class GetEarningsSummaryResponse
{
    public bool Success { get; set; }
    public EarningsSummaryData? Data { get; set; }
}

public class EarningsSummaryData
{
    public decimal TotalEarnings { get; set; }
    public decimal ThisMonth { get; set; }
    public decimal LastMonth { get; set; }
    public decimal PendingPayouts { get; set; }
    public int TotalTransactions { get; set; }
}

