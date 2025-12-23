using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.VendorBookings;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.VendorBookings;

public class ApproveBookingHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ApproveBookingHandler(
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ApproveBookingResponse> HandleAsync(long userId, long bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
        if (booking == null)
        {
            return new ApproveBookingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Booking not found" }
            };
        }

        if (booking.VendorId != userId)
        {
            return new ApproveBookingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "FORBIDDEN", Message = "Not authorized" }
            };
        }

        booking.Status = BookingStatus.Confirmed;
        booking.VendorStatus = "APPROVED";
        booking.VendorRespondedAt = _dateTimeProvider.GetUtcNow();
        booking.ConfirmedAt = _dateTimeProvider.GetUtcNow();
        booking.UpdatedAt = _dateTimeProvider.GetUtcNow();

        booking.Timeline.Add(new Domain.Entities.BookingTimeline
        {
            StatusFrom = BookingStatus.Pending.ToString(),
            StatusTo = BookingStatus.Confirmed.ToString(),
            ChangedBy = userId,
            Notes = "Booking approved by vendor",
            CreatedAt = _dateTimeProvider.GetUtcNow()
        });

        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new ApproveBookingResponse
        {
            Success = true,
            Data = new ApproveBookingResponseData
            {
                BookingId = booking.BookingReference,
                Status = "CONFIRMED",
                Message = "Booking approved successfully"
            }
        };
    }
}

