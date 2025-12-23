using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.VendorEarnings;

public class GetPayoutHistoryResponse
{
    public bool Success { get; set; }
    public List<PayoutHistoryDto> Data { get; set; } = [];
    public PaginationInfo? Pagination { get; set; }
}

public class PayoutHistoryDto
{
    public string Id { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Date { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
}

