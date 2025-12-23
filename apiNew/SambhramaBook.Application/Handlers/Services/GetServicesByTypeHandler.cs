using SambhramaBook.Application.Common;
using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Services;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Services;

public interface IGetServicesByTypeHandler : IQueryHandler<GetServicesByTypeRequest, GetServicesByTypeResponse>;

public class GetServicesByTypeHandler : IGetServicesByTypeHandler
{
    private readonly IServiceQueries _serviceQueries;

    public GetServicesByTypeHandler(IServiceQueries serviceQueries)
    {
        _serviceQueries = serviceQueries;
    }

    public async Task<GetServicesByTypeResponse> Handle(GetServicesByTypeRequest request, CancellationToken ct)
    {
        // ListingType only has Hall and Service, so all services use Service type
        var listingType = ListingType.Service;

        var (listings, total) = await _serviceQueries.GetServicesByTypeAsync(
            listingType,
            request.Location,
            request.Page,
            request.PageSize,
            ct);

        var serviceDtos = listings.Select(l => new ServiceListItemDto
        {
            Id = l.Id,
            ServiceType = (int)l.ListingType,
            Title = l.Title,
            Description = l.ShortDescription,
            City = l.City,
            Location = $"{l.AddressLine1}, {l.City}",
            Latitude = l.Latitude,
            Longitude = l.Longitude,
            Rating = l.AverageRating,
            ReviewCount = l.TotalReviews,
            Price = l.BasePrice,
            ImageUrl = l.Images.FirstOrDefault(img => img.IsPrimary)?.ImageUrl,
            Images = l.Images.OrderBy(img => img.DisplayOrder).Select(img => img.ImageUrl).ToList()
        }).ToList();

        return new GetServicesByTypeResponse
        {
            Success = true,
            Data = serviceDtos,
            Pagination = new PaginationInfo
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Total = total,
                TotalPages = (int)Math.Ceiling(total / (double)request.PageSize)
            }
        };
    }
}

