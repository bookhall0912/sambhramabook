using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain;

public sealed class Amenity
{
    public Guid Id { get; set; }

    public required string Code { get; set; }          // AC, PARKING, DRONE
    public required string DisplayName { get; set; }   // Air Conditioning
    public string? IconUrl { get; set; }

    public ServiceType Scope { get; set; }             // Hall, Photography, All

    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public ICollection<ServiceAmenity> ServiceAmenities { get; set; } = [];
}

