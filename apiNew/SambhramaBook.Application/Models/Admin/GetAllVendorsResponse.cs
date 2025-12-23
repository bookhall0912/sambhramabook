using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class GetAllVendorsResponse
{
    public bool Success { get; set; }
    public List<AdminVendorDto> Data { get; set; } = [];
    public PaginationInfo? Pagination { get; set; }
}

public class AdminVendorDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public string? Avatar { get; set; }
    public string Status { get; set; } = string.Empty;
    public string JoinDate { get; set; } = string.Empty;
    public int Listings { get; set; }
    public decimal Earnings { get; set; }
    public string VerificationStatus { get; set; } = string.Empty;
}

