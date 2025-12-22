namespace SambhramaBook.Domain;

public sealed class PhotographyServiceDetail
{
    public Guid ServiceId { get; set; }
    public int ExperienceYears { get; set; }
    public decimal StartingPrice { get; set; }
    public string? EquipmentJson { get; set; }

    public Service Service { get; set; } = null!;
}

