using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.VendorListings;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.VendorListings;

public class UpdateListingStatusHandler
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateListingStatusHandler(
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<UpdateListingStatusResponse> HandleAsync(long userId, long listingId, UpdateListingStatusRequest request, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId, cancellationToken);
        if (listing == null || listing.VendorId != userId)
        {
            return new UpdateListingStatusResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Listing not found" }
            };
        }

        listing.Status = request.Status switch
        {
            "DRAFT" => ListingStatus.Draft,
            "ACTIVE" => ListingStatus.Approved,
            "INACTIVE" => ListingStatus.Inactive,
            _ => listing.Status
        };
        listing.UpdatedAt = _dateTimeProvider.GetUtcNow();

        await _listingRepository.UpdateAsync(listing, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new UpdateListingStatusResponse
        {
            Success = true,
            Data = new UpdateListingStatusResponseData
            {
                Id = listing.Id.ToString(),
                Status = request.Status,
                Message = "Status updated successfully"
            }
        };
    }
}

