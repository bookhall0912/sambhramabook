namespace SambhramaBook.Application.Models.Hall;

public class HallListItemDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty; // Alias for Title to match API spec
    public string Title { get; set; } = string.Empty; // Keep for backward compatibility
    public string Location { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string? Distance { get; set; } // Changed to string to match spec (e.g., "2.5 km")
    public decimal Rating { get; set; } // Alias for AverageRating
    public decimal AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public int Capacity { get; set; } // Main capacity (maxCapacity or capacityMax)
    public int? MinCapacity { get; set; }
    public int? MaxCapacity { get; set; }
    public int? CapacityMin { get; set; } // Keep for backward compatibility
    public int? CapacityMax { get; set; } // Keep for backward compatibility
    public int? Rooms { get; set; }
    public decimal Price { get; set; } // Alias for BasePrice
    public decimal BasePrice { get; set; }
    public string? ImageUrl { get; set; }
    public List<string> Images { get; set; } = []; // Array of image URLs
    public List<string> Amenities { get; set; } = [];
    public string? Description { get; set; }
    public string? Parking { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}

