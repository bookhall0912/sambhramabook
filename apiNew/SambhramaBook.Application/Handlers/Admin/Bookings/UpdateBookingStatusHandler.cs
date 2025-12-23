using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Bookings;

public class UpdateBookingStatusHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateBookingStatusHandler(
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<UpdateBookingStatusResponse> HandleAsync(long bookingId, UpdateBookingStatusRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
        if (booking == null)
        {
            return new UpdateBookingStatusResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Booking not found" }
            };
        }

        booking.Status = Enum.Parse<BookingStatus>(request.Status, ignoreCase: true);
        booking.UpdatedAt = _dateTimeProvider.GetUtcNow();

        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new UpdateBookingStatusResponse
        {
            Success = true,
            Data = new UpdateBookingStatusResponseData
            {
                Id = booking.Id.ToString(),
                Status = request.Status,
                Message = "Booking status updated"
            }
        };
    }
}

