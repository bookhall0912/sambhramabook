namespace SambhramaBook.Application.Models.Admin;

public class GetPendingListingsResponse
{
    public bool Success { get; set; }
    public List<PendingListingDto> Data { get; set; } = [];
}

