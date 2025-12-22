using System.Collections.ObjectModel;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Service;

public sealed class ServiceGetHandler : IServicesGetHandler
{
    private readonly IServiceQueries _serviceQueries;

    public ServiceGetHandler(IServiceQueries serviceQueries)
    {
        _serviceQueries = serviceQueries;
    }

    public async Task<ReadOnlyCollection<ServiceResponseDto>> Handle(ServiceGetModel model, CancellationToken ct)
    {
        ReadOnlyCollection<ServiceWithDetails> servicesWithDetails = await _serviceQueries.GetServices(model, ct);
        return servicesWithDetails.Select(CalculateServiceData).ToList().AsReadOnly();
    }

    private static ServiceResponseDto CalculateServiceData(ServiceWithDetails serviceWithDetails)
    {
        Domain.Service service = serviceWithDetails.Service;
        HallServiceDetail? hallDetails = serviceWithDetails.HallServiceDetails;
        IReadOnlyList<Review> reviews = serviceWithDetails.Reviews;
        IReadOnlyList<ServiceMedia> serviceMedia = serviceWithDetails.ServiceMedia;
        IReadOnlyList<Amenity> amenities = serviceWithDetails.Amenities;

        // Calculate rating and review count
        double rating = reviews.Count > 0 ? Math.Round(reviews.Average(r => r.Rating), 1) : 0.0;
        int reviewCount = reviews.Count;

        // Get cover image
        ServiceMedia? coverImage = serviceMedia.FirstOrDefault(sm => sm.IsCover && sm.MediaType == MediaType.Image);
        string? imageUrl = coverImage?.MediaUrl;

        // Get all images
        List<string> images = serviceMedia
            .Where(sm => sm.MediaType == MediaType.Image)
            .Select(sm => sm.MediaUrl)
            .ToList();

        // Get amenities codes
        List<string> amenitiesCodes = amenities.Select(a => a.Code).ToList();

        // Get parking info from amenities
        Amenity? parkingAmenity = amenities.FirstOrDefault(a =>
            a.Code.Equals("PARKING", StringComparison.OrdinalIgnoreCase));
        string? parking = parkingAmenity?.DisplayName;

        // Get capacity, price, rooms from hall details
        int capacity = hallDetails?.Capacity ?? 0;
        int? minCapacity = hallDetails?.MinCapacity;
        int? maxCapacity = hallDetails?.MaxCapacity;
        int rooms = hallDetails?.Rooms ?? 0;
        decimal price = hallDetails?.PricePerDay ?? service.SearchPrice;

        // Location (currently just city, can be extended later)
        string location = service.City;

        return new ServiceResponseDto
        {
            Id = service.Id,
            ServiceType = service.ServiceType,
            Title = service.Title,
            Description = service.Description,
            City = service.City,
            Latitude = service.Latitude,
            Longitude = service.Longitude,
            Rating = rating,
            ReviewCount = reviewCount,
            Capacity = capacity,
            MinCapacity = minCapacity,
            MaxCapacity = maxCapacity,
            Rooms = rooms,
            Price = price,
            ImageUrl = imageUrl,
            Images = images,
            Amenities = amenitiesCodes,
            Parking = parking,
            Location = location
        };
    }
}
