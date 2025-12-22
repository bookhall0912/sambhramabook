namespace SambhramaBook.Domain;

public sealed class ServiceAvailability
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public DateOnly Date { get; set; }
    public bool IsAvailable { get; set; }

    public Service Service { get; set; } = null!;
}

