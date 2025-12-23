using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Listings;

public class ApproveListingHandler
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ApproveListingHandler(
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ApproveListingResponse> HandleAsync(long listingId, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId, cancellationToken);
        if (listing == null)
        {
            return new ApproveListingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Listing not found" }
            };
        }

        listing.ApprovalStatus = ApprovalStatus.Approved;
        listing.Status = ListingStatus.Approved;
        listing.UpdatedAt = _dateTimeProvider.GetUtcNow();

        await _listingRepository.UpdateAsync(listing, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new ApproveListingResponse
        {
            Success = true,
            Data = new ApproveListingResponseData
            {
                Id = listing.Id.ToString(),
                Status = "APPROVED",
                Message = "Listing approved successfully"
            }
        };
    }
}

