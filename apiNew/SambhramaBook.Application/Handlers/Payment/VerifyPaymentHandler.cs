using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Payment;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Payment;

public class VerifyPaymentHandler
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public VerifyPaymentHandler(
        IPaymentRepository paymentRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _paymentRepository = paymentRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<VerifyPaymentResponse> HandleAsync(VerifyPaymentRequest request, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByPaymentReferenceAsync(request.PaymentId, cancellationToken);
        if (payment == null)
        {
            return new VerifyPaymentResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Payment not found" }
            };
        }

        // TODO: Verify signature with payment gateway
        // For now, mark as successful if signature is provided
        Domain.Entities.Booking? booking = null;
        if (!string.IsNullOrEmpty(request.Signature))
        {
            payment.Status = PaymentStatus.Paid;
            payment.GatewayTransactionId = request.PaymentId;
            payment.PaidAt = _dateTimeProvider.GetUtcNow();
            payment.UpdatedAt = _dateTimeProvider.GetUtcNow();

            // Update booking payment status
            booking = await _bookingRepository.GetByIdAsync(payment.BookingId, cancellationToken);
            if (booking != null)
            {
                booking.PaymentStatus = PaymentStatus.Paid;
                booking.UpdatedAt = _dateTimeProvider.GetUtcNow();
                await _bookingRepository.UpdateAsync(booking, cancellationToken);
            }

            await _paymentRepository.UpdateAsync(payment, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);
        }

        return new VerifyPaymentResponse
        {
            Success = true,
            Data = new VerifyPaymentResponseData
            {
                PaymentId = payment.PaymentReference,
                Status = payment.Status == PaymentStatus.Paid ? "success" : "pending",
                Amount = payment.Amount,
                BookingId = booking?.BookingReference ?? string.Empty
            }
        };
    }
}

