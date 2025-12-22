namespace SambhramaBook.Application.Models.Hall;

public class HallAvailabilityResponseDto
{
    public string Month { get; set; } = string.Empty;
    public int Year { get; set; }
    public List<AvailabilityDayDto> Days { get; set; } = [];
}

public class AvailabilityDayDto
{
    public int Day { get; set; }
    public string Status { get; set; } = string.Empty; // available, booked, unavailable
    public decimal? Price { get; set; }
}

