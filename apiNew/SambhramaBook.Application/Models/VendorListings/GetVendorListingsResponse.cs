using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.VendorListings;

public class GetVendorListingsResponse
{
    public bool Success { get; set; }
    public List<VendorListingDto> Data { get; set; } = [];
    public PaginationInfo? Pagination { get; set; }
}

public class VendorListingDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // ACTIVE | DRAFT | INACTIVE
    public string Type { get; set; } = string.Empty; // Hall | Service
}

