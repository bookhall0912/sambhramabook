using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.VendorEarnings;

public class GetEarningsTransactionsResponse
{
    public bool Success { get; set; }
    public List<EarningsTransactionDto> Data { get; set; } = [];
    public PaginationInfo? Pagination { get; set; }
}

public class EarningsTransactionDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Commission { get; set; }
    public decimal NetAmount { get; set; }
    public string Date { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

