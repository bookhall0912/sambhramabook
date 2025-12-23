using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Booking;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Booking;

public class CancelBookingHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CancelBookingHandler(
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<CancelBookingResponse> HandleAsync(long userId, long bookingId, CancelBookingRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
        if (booking == null)
        {
            return new CancelBookingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Booking not found" }
            };
        }

        // Check authorization - only customer can cancel
        if (booking.CustomerId != userId)
        {
            return new CancelBookingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "FORBIDDEN", Message = "Not authorized to cancel this booking" }
            };
        }

        if (booking.Status == BookingStatus.Cancelled)
        {
            return new CancelBookingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "ALREADY_CANCELLED", Message = "Booking is already cancelled" }
            };
        }

        // Calculate refund (50% if cancelled more than 30 days before, 0% if less)
        var daysUntilEvent = (booking.StartDate.ToDateTime(TimeOnly.MinValue) - _dateTimeProvider.GetUtcNow()).Days;
        var refundPercentage = daysUntilEvent > 30 ? 0.5m : 0m;
        var refundAmount = booking.TotalAmount * refundPercentage;

        booking.Status = BookingStatus.Cancelled;
        booking.CancellationReason = request.Reason;
        booking.CancelledAt = _dateTimeProvider.GetUtcNow();
        booking.CancelledBy = userId;
        booking.RefundAmount = refundAmount;
        booking.UpdatedAt = _dateTimeProvider.GetUtcNow();

        booking.Timeline.Add(new Domain.Entities.BookingTimeline
        {
            StatusFrom = booking.Status.ToString(),
            StatusTo = BookingStatus.Cancelled.ToString(),
            ChangedBy = userId,
            Notes = $"Cancelled: {request.Reason}",
            CreatedAt = _dateTimeProvider.GetUtcNow()
        });

        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new CancelBookingResponse
        {
            Success = true,
            Data = new CancelBookingResponseData
            {
                BookingId = booking.BookingReference,
                Status = "CANCELLED",
                RefundAmount = refundAmount,
                Message = "Booking cancelled successfully"
            }
        };
    }
}

