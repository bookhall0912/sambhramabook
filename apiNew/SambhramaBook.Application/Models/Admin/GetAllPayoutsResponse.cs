using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class GetAllPayoutsResponse
{
    public bool Success { get; set; }
    public List<AdminPayoutDto> Data { get; set; } = [];
    public PaginationInfo? Pagination { get; set; }
}

public class AdminPayoutDto
{
    public string Id { get; set; } = string.Empty;
    public string VendorId { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string RequestDate { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? ProcessedDate { get; set; }
}

