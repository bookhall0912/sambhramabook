using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Handlers.Service;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Infrastructure.Queries;

public sealed class ServiceQueries : IServiceQueries
{
    private readonly SambhramaBookDbContext _dbContext;
    private const double EarthRadiusKm = 6371;

    public ServiceQueries(SambhramaBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ReadOnlyCollection<ServiceCategory>> GetCategories(CancellationToken ct) =>
        (await _dbContext.ServiceCategories
            .Where(c => c.DisplayOrder > 0)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync(ct)).AsReadOnly();

    public async Task<ReadOnlyCollection<ServiceWithDetails>> GetServices(ServiceGetModel model, CancellationToken ct)
    {
        var listings = from s in _dbContext.Services.AsNoTracking()
                       where s.Status == ServiceStatus.Live
                             && s.ServiceType == ServiceType.Hall

                       join hsd in _dbContext.HallServiceDetails.AsNoTracking()
                           on s.Id equals hsd.ServiceId

                       // Reviews (LEFT JOIN + aggregation)
                       join r in _dbContext.Reviews.AsNoTracking()
                           on s.Id equals r.ServiceId into reviews

                       let rating = reviews.Any()
                           ? reviews.Average(x => x.Rating)
                           : 0

                       let reviewCount = reviews.Count()

                       // Cover Image
                       join sm in _dbContext.ServiceMedias.AsNoTracking()
                               .Where(x => x.IsCover)
                           on s.Id equals sm.ServiceId into covers

                       let coverImageUrl = covers
                           .Select(x => x.MediaUrl)
                           .FirstOrDefault()

                       // Amenities
                       join sa in _dbContext.ServiceAmenities.AsNoTracking()
                           on s.Id equals sa.ServiceId into amenityGroup

                       select new ServiceListingResponse
                       {
                           Id = s.Id,
                           ServiceType = s.ServiceType,
                           Title = s.Title,
                           Description = s.Description,
                           City = s.City,

                           Location = s.FullAddress ?? s.City,

                           Rating = Math.Round(rating, 1),
                           ReviewCount = reviewCount,

                           MinCapacity = hsd.MinCapacity,
                           MaxCapacity = hsd.MaxCapacity,
                           Capacity = hsd.MaxCapacity,
                           Rooms = hsd.Rooms,
                           Parking = hsd.ParkingCapacity != null
                               ? $"{hsd.ParkingCapacity}+ Cars Parking"
                               : null,

                           Price = hsd.PricePerDay,

                           ImageUrl = coverImageUrl ?? string.Empty,

                           Amenities = amenityGroup
                               .Select(x => x.Amenity.Code)
                               .ToList(),

                           Latitude = s.Latitude,
                           Longitude = s.Longitude
                       };

        List<ServiceWithDetails> results = await listings.Take(model.Limit).ToListAsync(ct);

        return results.AsReadOnly();
    }

    private static double ToRadians(double degrees) => degrees * (Math.PI / 180);
}
