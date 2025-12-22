namespace SambhramaBook.Domain;

public sealed class HallServiceDetails
{
    public Guid ServiceId { get; set; }
    public int Capacity { get; set; }
    public int? MinCapacity { get; set; }
    public int? MaxCapacity { get; set; }
    public int Rooms { get; set; }
    public decimal PricePerDay { get; set; }
    public string? AmenitiesJson { get; set; }
    public bool ParkingAvailable { get; set; }
    public string? CancellationPolicy { get; set; }

    public Service Service { get; set; } = null!;
}

