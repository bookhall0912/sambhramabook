namespace SambhramaBook.Domain;

public sealed class EventManagementServiceDetail
{
    public Guid ServiceId { get; set; }
    public string? EventTypesJson { get; set; }
    public int TeamSize { get; set; }
    public decimal StartingPrice { get; set; }

    public Service Service { get; set; } = null!;
}

