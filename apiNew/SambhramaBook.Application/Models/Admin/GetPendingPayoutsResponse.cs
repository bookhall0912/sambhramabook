namespace SambhramaBook.Application.Models.Admin;

public class GetPendingPayoutsResponse
{
    public bool Success { get; set; }
    public List<PendingPayoutDto> Data { get; set; } = [];
}

public class PendingPayoutDto
{
    public string Id { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string RequestDate { get; set; } = string.Empty;
}

