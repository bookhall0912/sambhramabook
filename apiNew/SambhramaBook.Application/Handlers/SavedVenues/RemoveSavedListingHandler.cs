using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.SavedVenues;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;

namespace SambhramaBook.Application.Handlers.SavedVenues;

public class RemoveSavedListingHandler
{
    private readonly ISavedListingRepository _savedListingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveSavedListingHandler(
        ISavedListingRepository savedListingRepository,
        IUnitOfWork unitOfWork)
    {
        _savedListingRepository = savedListingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RemoveSavedListingResponse> HandleAsync(long userId, long listingId, CancellationToken cancellationToken = default)
    {
        var savedListing = await _savedListingRepository.GetByUserAndListingAsync(userId, listingId, cancellationToken);
        if (savedListing == null)
        {
            return new RemoveSavedListingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Saved listing not found" }
            };
        }

        await _savedListingRepository.DeleteAsync(savedListing.Id, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new RemoveSavedListingResponse
        {
            Success = true,
            Message = "Listing removed from saved"
        };
    }
}

