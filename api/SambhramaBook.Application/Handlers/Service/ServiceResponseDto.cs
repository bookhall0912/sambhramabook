using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Service;

public sealed class ServiceResponseDto
{
    public Guid Id { get; init; }
    public ServiceType ServiceType { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public string City { get; init; } = null!;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double Rating { get; init; }
    public int ReviewCount { get; init; }
    public int Capacity { get; init; }
    public int? MinCapacity { get; init; }
    public int? MaxCapacity { get; init; }
    public int Rooms { get; init; }
    public decimal Price { get; init; }
    public string? ImageUrl { get; init; }
    public IReadOnlyList<string> Images { get; init; } = [];
    public IReadOnlyList<string> Amenities { get; init; } = [];
    public string? Parking { get; init; }
    public string Location { get; init; } = null!;
}

