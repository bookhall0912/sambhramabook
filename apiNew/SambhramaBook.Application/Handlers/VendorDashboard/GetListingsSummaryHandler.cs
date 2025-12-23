using SambhramaBook.Application.Models.VendorDashboard;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.VendorDashboard;

public interface IGetListingsSummaryHandler
{
    Task<GetListingsSummaryResponse> Handle(long userId, CancellationToken ct);
}

public class GetListingsSummaryHandler : IGetListingsSummaryHandler
{
    private readonly IListingRepository _listingRepository;

    public GetListingsSummaryHandler(
        IListingRepository listingRepository)
    {
        _listingRepository = listingRepository;
    }

    public async Task<GetListingsSummaryResponse> Handle(long userId, CancellationToken ct)
    {
        var listings = await _listingRepository.GetByVendorIdAsync(userId, ct);
        var listingDtos = listings.Select(l => new ListingSummaryDto
        {
            Id = l.Id.ToString(),
            Name = l.Title,
            Image = l.Images.FirstOrDefault(img => img.IsPrimary)?.ImageUrl,
            Location = l.City,
            Status = l.Status == ListingStatus.Approved ? "ACTIVE" :
                    l.Status == ListingStatus.Draft ? "DRAFT" : "INACTIVE"
        }).ToList();

        return new GetListingsSummaryResponse
        {
            Success = true,
            Data = listingDtos
        };
    }
}

