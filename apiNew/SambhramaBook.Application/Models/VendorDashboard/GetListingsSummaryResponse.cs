namespace SambhramaBook.Application.Models.VendorDashboard;

public class GetListingsSummaryResponse
{
    public bool Success { get; set; }
    public List<ListingSummaryDto> Data { get; set; } = [];
}

