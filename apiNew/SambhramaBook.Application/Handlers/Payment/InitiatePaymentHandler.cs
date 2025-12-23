using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Payment;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Payment;

public class InitiatePaymentHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public InitiatePaymentHandler(
        IBookingRepository bookingRepository,
        IPaymentRepository paymentRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _bookingRepository = bookingRepository;
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<InitiatePaymentResponse> HandleAsync(long userId, InitiatePaymentRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByReferenceAsync(request.BookingId, cancellationToken);
        if (booking == null)
        {
            return new InitiatePaymentResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Booking not found" }
            };
        }

        if (booking.CustomerId != userId)
        {
            return new InitiatePaymentResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "FORBIDDEN", Message = "Not authorized" }
            };
        }

        var payment = new Domain.Entities.Payment
        {
            BookingId = booking.Id,
            PaymentReference = $"PAY-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..20],
            Amount = request.Amount,
            Currency = "INR",
            PaymentMethod = request.PaymentMethod,
            PaymentGateway = request.PaymentMethod.ToUpper(),
            Status = PaymentStatus.Pending,
            CreatedAt = _dateTimeProvider.GetUtcNow(),
            UpdatedAt = _dateTimeProvider.GetUtcNow()
        };

        payment = await _paymentRepository.CreateAsync(payment, cancellationToken);

        // TODO: Integrate with payment gateway (Razorpay, Stripe, etc.)
        var paymentUrl = $"https://checkout.{request.PaymentMethod.ToLower()}.com/pay/{payment.PaymentReference}";

        await _unitOfWork.SaveChanges(cancellationToken);

        return new InitiatePaymentResponse
        {
            Success = true,
            Data = new InitiatePaymentResponseData
            {
                PaymentId = payment.PaymentReference,
                OrderId = payment.Id.ToString(),
                Amount = payment.Amount,
                PaymentUrl = paymentUrl,
                Status = "pending"
            }
        };
    }
}

