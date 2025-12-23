using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Booking;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Booking;

public class RescheduleBookingHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RescheduleBookingHandler(
        IBookingRepository bookingRepository,
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _bookingRepository = bookingRepository;
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<RescheduleBookingResponse> HandleAsync(long userId, long bookingId, RescheduleBookingRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
        if (booking == null)
        {
            return new RescheduleBookingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Booking not found" }
            };
        }

        if (booking.CustomerId != userId)
        {
            return new RescheduleBookingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "FORBIDDEN", Message = "Not authorized" }
            };
        }

        // Check new dates availability
        var isAvailable = await CheckAvailabilityAsync(booking.ListingId, request.NewStartDate, request.NewEndDate, bookingId, cancellationToken);
        if (!isAvailable)
        {
            return new RescheduleBookingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "UNAVAILABLE", Message = "New dates are not available" }
            };
        }

        var numberOfDays = (request.NewEndDate.ToDateTime(TimeOnly.MinValue) - request.NewStartDate.ToDateTime(TimeOnly.MinValue)).Days + 1;

        booking.StartDate = request.NewStartDate;
        booking.EndDate = request.NewEndDate;
        booking.DurationDays = numberOfDays;
        booking.UpdatedAt = _dateTimeProvider.GetUtcNow();

        booking.Timeline.Add(new Domain.Entities.BookingTimeline
        {
            StatusFrom = booking.Status.ToString(),
            StatusTo = booking.Status.ToString(),
            ChangedBy = userId,
            Notes = $"Reschedule requested: {request.Reason}",
            CreatedAt = _dateTimeProvider.GetUtcNow()
        });

        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new RescheduleBookingResponse
        {
            Success = true,
            Data = new RescheduleBookingResponseData
            {
                BookingId = booking.BookingReference,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Message = "Reschedule request submitted"
            }
        };
    }

    private async Task<bool> CheckAvailabilityAsync(long listingId, DateOnly startDate, DateOnly endDate, long excludeBookingId, CancellationToken cancellationToken)
    {
        var existingBookings = await _bookingRepository.GetByListingIdAsync(listingId, cancellationToken);
        var hasConflict = existingBookings.Any(b => 
            b.Id != excludeBookingId &&
            b.Status != BookingStatus.Cancelled &&
            b.StartDate <= endDate && 
            b.EndDate >= startDate);
        
        return !hasConflict;
    }
}

