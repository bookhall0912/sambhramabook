namespace SambhramaBook.Application.Models.SavedVenues;

public class GetSavedListingsResponse
{
    public bool Success { get; set; }
    public List<SavedListingDto> Data { get; set; } = [];
}

public class SavedListingDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public int ReviewCount { get; set; }
}

