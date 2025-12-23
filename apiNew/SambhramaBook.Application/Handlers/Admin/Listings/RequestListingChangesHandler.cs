using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Listings;

public class RequestListingChangesHandler
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RequestListingChangesHandler(
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<RequestListingChangesResponse> HandleAsync(long listingId, RequestListingChangesRequest request, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId, cancellationToken);
        if (listing == null)
        {
            return new RequestListingChangesResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Listing not found" }
            };
        }

        listing.ApprovalStatus = ApprovalStatus.NeedsChanges;
        listing.UpdatedAt = _dateTimeProvider.GetUtcNow();
        // TODO: Store change request notes

        await _listingRepository.UpdateAsync(listing, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new RequestListingChangesResponse
        {
            Success = true,
            Data = new RequestListingChangesResponseData
            {
                Id = listing.Id.ToString(),
                Status = "NEEDS_CHANGES",
                Message = "Changes requested"
            }
        };
    }
}

