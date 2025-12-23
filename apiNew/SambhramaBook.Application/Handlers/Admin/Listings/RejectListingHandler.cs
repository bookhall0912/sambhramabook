using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Listings;

public class RejectListingHandler
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RejectListingHandler(
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<RejectListingResponse> HandleAsync(long listingId, RejectListingRequest request, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId, cancellationToken);
        if (listing == null)
        {
            return new RejectListingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Listing not found" }
            };
        }

        listing.ApprovalStatus = ApprovalStatus.Rejected;
        listing.Status = ListingStatus.Inactive;
        listing.UpdatedAt = _dateTimeProvider.GetUtcNow();
        // TODO: Store rejection notes in a separate table or field

        await _listingRepository.UpdateAsync(listing, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new RejectListingResponse
        {
            Success = true,
            Data = new RejectListingResponseData
            {
                Id = listing.Id.ToString(),
                Status = "REJECTED",
                Message = "Listing rejected"
            }
        };
    }
}

