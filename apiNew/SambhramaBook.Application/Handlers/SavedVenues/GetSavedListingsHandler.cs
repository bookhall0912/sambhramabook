using SambhramaBook.Application.Models.SavedVenues;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.SavedVenues;

public interface IGetSavedListingsHandler
{
    Task<GetSavedListingsResponse> Handle(long userId, CancellationToken ct);
}

public class GetSavedListingsHandler : IGetSavedListingsHandler
{
    private readonly ISavedListingQueries _savedListingQueries;

    public GetSavedListingsHandler(ISavedListingQueries savedListingQueries)
    {
        _savedListingQueries = savedListingQueries;
    }

    public async Task<GetSavedListingsResponse> Handle(long userId, CancellationToken ct)
    {
        var (savedListings, _) = await _savedListingQueries.GetSavedListingsAsync(
            userId,
            1,
            int.MaxValue,
            ct);

        var listingDtos = savedListings.Select(sl => new SavedListingDto
        {
            Id = sl.ListingId.ToString(),
            Name = sl.Listing.Title,
            Image = sl.Listing.Images.FirstOrDefault(img => img.IsPrimary)?.ImageUrl,
            Location = $"{sl.Listing.City}, {sl.Listing.State}",
            Rating = sl.Listing.AverageRating,
            ReviewCount = sl.Listing.TotalReviews
        }).ToList();

        return new GetSavedListingsResponse
        {
            Success = true,
            Data = listingDtos
        };
    }
}

