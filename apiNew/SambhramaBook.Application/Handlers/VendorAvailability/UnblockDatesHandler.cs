using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.VendorAvailability;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;

namespace SambhramaBook.Application.Handlers.VendorAvailability;

public class UnblockDatesHandler
{
    private readonly IListingRepository _listingRepository;
    private readonly IListingAvailabilityRepository _availabilityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UnblockDatesHandler(
        IListingRepository listingRepository,
        IListingAvailabilityRepository availabilityRepository,
        IUnitOfWork unitOfWork)
    {
        _listingRepository = listingRepository;
        _availabilityRepository = availabilityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnblockDatesResponse> HandleAsync(long userId, long listingId, UnblockDatesRequest request, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId, cancellationToken);
        if (listing == null || listing.VendorId != userId)
        {
            return new UnblockDatesResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Listing not found" }
            };
        }

        var dates = request.Dates.Select(DateOnly.Parse).ToList();
        var availabilities = await _availabilityRepository.GetByListingIdAsync(listingId, cancellationToken);
        var toDelete = availabilities.Where(la => dates.Contains(la.Date)).ToList();

        foreach (var availability in toDelete)
        {
            await _availabilityRepository.DeleteAsync(availability.Id, cancellationToken);
        }

        await _unitOfWork.SaveChanges(cancellationToken);

        return new UnblockDatesResponse
        {
            Success = true,
            Message = "Dates unblocked successfully"
        };
    }
}

