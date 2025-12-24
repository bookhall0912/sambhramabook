using SambhramaBook.Application.Models.Services;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Queries;

public interface IServiceQueries
{
    Task<List<ServiceCategoryDto>> GetServiceCategoriesAsync(CancellationToken cancellationToken = default);
    Task<Listing?> GetServiceDetailsWithIncludesAsync(long id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Listing> Listings, int Total)> GetServicesByTypeAsync(
        ListingType listingType,
        string? location,
        string? categoryCode,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}

