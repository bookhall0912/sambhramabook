using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain;

public sealed class ServiceMedia
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public required string MediaUrl { get; set; }
    public MediaType MediaType { get; set; }
    public bool IsCover { get; set; }

    public Service Service { get; set; } = null!;
}

