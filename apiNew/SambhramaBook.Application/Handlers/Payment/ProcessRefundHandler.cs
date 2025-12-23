using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Payment;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Payment;

public class ProcessRefundHandler
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ProcessRefundHandler(
        IPaymentRepository paymentRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ProcessRefundResponse> HandleAsync(long userId, long paymentId, ProcessRefundRequest request, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId, cancellationToken);
        if (payment == null)
        {
            return new ProcessRefundResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Payment not found" }
            };
        }

        var refundAmount = request.Amount > 0 ? request.Amount : payment.Amount;
        var refundReference = $"REF-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..20];

        // TODO: Process refund with payment gateway
        payment.Status = PaymentStatus.Refunded;
        payment.RefundAmount = refundAmount;
        payment.RefundReason = request.Reason;
        payment.RefundedAt = _dateTimeProvider.GetUtcNow();
        payment.UpdatedAt = _dateTimeProvider.GetUtcNow();

        await _paymentRepository.UpdateAsync(payment, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new ProcessRefundResponse
        {
            Success = true,
            Data = new ProcessRefundResponseData
            {
                RefundId = refundReference,
                Amount = refundAmount,
                Status = "processed"
            }
        };
    }
}

