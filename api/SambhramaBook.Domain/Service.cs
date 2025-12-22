using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain;

public sealed class Service
{
    public Guid Id { get; set; }
    public Guid VendorId { get; set; }
    public ServiceType ServiceType { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string City { get; set; }
    public string? FullAddress { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public decimal SearchPrice { get; set; }
    public ServiceStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public Vendor Vendor { get; set; } = null!;
    public HallServiceDetail? HallServiceDetails { get; set; }
    public PhotographyServiceDetail? PhotographyServiceDetails { get; set; }
    public CateringServiceDetail? CateringServiceDetails { get; set; }
    public EventManagementServiceDetail? EventManagementServiceDetails { get; set; }
    public ICollection<ServiceMedia> ServiceMedia { get; set; } = [];
    public ICollection<ServiceAvailability> ServiceAvailabilities { get; set; } = [];
    public ICollection<ServiceAmenity> ServiceAmenities { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Conversation> Conversations { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
}

