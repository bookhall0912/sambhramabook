namespace SambhramaBook.Application.Models.Hall;

public class HallSearchRequestDto
{
    public string? Location { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinCapacity { get; set; }
    public int? MaxCapacity { get; set; }
    public string? Amenities { get; set; } // Comma-separated
    public DateOnly? Date { get; set; }
    public int? Days { get; set; }
    public int? Guests { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}

