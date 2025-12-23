using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.VendorAvailability;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.VendorAvailability;

public class BlockDatesHandler
{
    private readonly IListingRepository _listingRepository;
    private readonly IListingAvailabilityRepository _availabilityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public BlockDatesHandler(
        IListingRepository listingRepository,
        IListingAvailabilityRepository availabilityRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _listingRepository = listingRepository;
        _availabilityRepository = availabilityRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<BlockDatesResponse> HandleAsync(long userId, long listingId, BlockDatesRequest request, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId, cancellationToken);
        if (listing == null || listing.VendorId != userId)
        {
            return new BlockDatesResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Listing not found" }
            };
        }

        foreach (var dateStr in request.Dates)
        {
            var date = DateOnly.Parse(dateStr);
            var existing = await _availabilityRepository.GetByListingIdAndDateAsync(listingId, date, cancellationToken);

            if (existing != null)
            {
                existing.Status = AvailabilityStatus.Blocked;
                existing.UpdatedAt = _dateTimeProvider.GetUtcNow();
                await _availabilityRepository.UpdateAsync(existing, cancellationToken);
            }
            else
            {
                var availability = new ListingAvailability
                {
                    ListingId = listingId,
                    Date = date,
                    Status = AvailabilityStatus.Blocked,
                    CreatedAt = _dateTimeProvider.GetUtcNow(),
                    UpdatedAt = _dateTimeProvider.GetUtcNow()
                };
                await _availabilityRepository.CreateAsync(availability, cancellationToken);
            }
        }

        await _unitOfWork.SaveChanges(cancellationToken);

        return new BlockDatesResponse
        {
            Success = true,
            Message = "Dates blocked successfully"
        };
    }
}

