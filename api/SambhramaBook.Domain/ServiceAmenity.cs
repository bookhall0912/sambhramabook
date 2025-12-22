namespace SambhramaBook.Domain;

public sealed class ServiceAmenity
{
    public Guid ServiceId { get; set; }
    public Service Service { get; set; } = null!;

    public Guid AmenityId { get; set; }
    public Amenity Amenity { get; set; } = null!;
}

