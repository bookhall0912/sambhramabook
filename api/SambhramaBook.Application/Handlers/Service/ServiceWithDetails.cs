using SambhramaBook.Domain;

namespace SambhramaBook.Application.Handlers.Service;

public sealed class ServiceWithDetails
{
    public Domain.Service Service { get; init; } = null!;
    public HallServiceDetail? HallServiceDetails { get; init; }
    public IReadOnlyList<Review> Reviews { get; init; } = [];
    public IReadOnlyList<ServiceMedia> ServiceMedia { get; init; } = [];
    public IReadOnlyList<Amenity> Amenities { get; init; } = [];
}

