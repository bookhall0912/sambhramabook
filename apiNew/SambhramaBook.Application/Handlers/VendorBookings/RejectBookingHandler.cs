using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.VendorBookings;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.VendorBookings;

public class RejectBookingHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RejectBookingHandler(
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<RejectBookingResponse> HandleAsync(long userId, long bookingId, RejectBookingRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
        if (booking == null)
        {
            return new RejectBookingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Booking not found" }
            };
        }

        if (booking.VendorId != userId)
        {
            return new RejectBookingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "FORBIDDEN", Message = "Not authorized" }
            };
        }

        booking.Status = BookingStatus.Cancelled;
        booking.VendorStatus = "REJECTED";
        booking.VendorResponseNotes = request.Reason;
        booking.VendorRespondedAt = _dateTimeProvider.GetUtcNow();
        booking.CancelledAt = _dateTimeProvider.GetUtcNow();
        booking.CancelledBy = userId;
        booking.CancellationReason = request.Reason;
        booking.UpdatedAt = _dateTimeProvider.GetUtcNow();

        booking.Timeline.Add(new Domain.Entities.BookingTimeline
        {
            StatusFrom = booking.Status.ToString(),
            StatusTo = BookingStatus.Cancelled.ToString(),
            ChangedBy = userId,
            Notes = $"Rejected: {request.Reason}",
            CreatedAt = _dateTimeProvider.GetUtcNow()
        });

        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new RejectBookingResponse
        {
            Success = true,
            Data = new RejectBookingResponseData
            {
                BookingId = booking.BookingReference,
                Status = "CANCELLED",
                Message = "Booking rejected"
            }
        };
    }
}

