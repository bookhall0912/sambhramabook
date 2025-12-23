using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.SavedVenues;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Handlers.SavedVenues;

public class SaveListingHandler
{
    private readonly ISavedListingRepository _savedListingRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public SaveListingHandler(
        ISavedListingRepository savedListingRepository,
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _savedListingRepository = savedListingRepository;
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<SaveListingResponse> HandleAsync(long userId, long listingId, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId, cancellationToken);
        if (listing == null)
        {
            return new SaveListingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Listing not found" }
            };
        }

        var existing = await _savedListingRepository.GetByUserAndListingAsync(userId, listingId, cancellationToken);
        if (existing != null)
        {
            return new SaveListingResponse
            {
                Success = true,
                Message = "Listing already saved"
            };
        }

        var savedListing = new SavedListing
        {
            UserId = userId,
            ListingId = listingId,
            CreatedAt = _dateTimeProvider.GetUtcNow()
        };

        await _savedListingRepository.CreateAsync(savedListing, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new SaveListingResponse
        {
            Success = true,
            Message = "Listing saved successfully"
        };
    }
}

