using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class GetPendingListingsForApprovalResponse
{
    public bool Success { get; set; }
    public List<AdminPendingListingDto> Data { get; set; } = [];
    public PaginationInfo? Pagination { get; set; }
}

public class AdminPendingListingDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public string? VendorAvatar { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Submitted { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

