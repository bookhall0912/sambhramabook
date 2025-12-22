using SambhramaBook.Domain.Enums;
using ServiceResponseDto = SambhramaBook.Application.Handlers.Service.ServiceResponseDto;

namespace SambhramaBook.Api.Features.Service;

public sealed class ServiceGetResponseModel
{
    public ServiceGetResponseModel(ServiceResponseDto dto)
    {
        Id = dto.Id;
        ServiceType = dto.ServiceType;
        Title = dto.Title;
        Description = dto.Description;
        City = dto.City;
        Rating = dto.Rating;
        ReviewCount = dto.ReviewCount;
        Capacity = dto.Capacity;
        MinCapacity = dto.MinCapacity;
        MaxCapacity = dto.MaxCapacity;
        Rooms = dto.Rooms;
        Price = dto.Price;
        ImageUrl = dto.ImageUrl;
        Images = dto.Images;
        Amenities = dto.Amenities;
        Parking = dto.Parking;
        Latitude = dto.Latitude;
        Longitude = dto.Longitude;
        Location = dto.Location;
    }

    public Guid Id { get; }
    public ServiceType ServiceType { get; }
    public string Title { get; }
    public string? Description { get; }
    public string City { get; }
    public double Rating { get; }
    public int ReviewCount { get; }
    public int Capacity { get; }
    public int? MinCapacity { get; }
    public int? MaxCapacity { get; }
    public int Rooms { get; }
    public decimal Price { get; }
    public string? ImageUrl { get; }
    public IReadOnlyList<string> Images { get; }
    public IReadOnlyList<string> Amenities { get; }
    public string? Parking { get; }
    public double Latitude { get; }
    public double Longitude { get; }
    public string Location { get; }
}
