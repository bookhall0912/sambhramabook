namespace SambhramaBook.Application.Models.Hall;

public class GetHallAvailabilityRequest
{
    public long HallId { get; set; }
    public string Month { get; set; } = string.Empty;
    public int Year { get; set; }
}

