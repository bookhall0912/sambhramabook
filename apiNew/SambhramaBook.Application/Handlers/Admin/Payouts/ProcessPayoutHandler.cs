using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Payouts;

public class ProcessPayoutHandler
{
    private readonly IPayoutRepository _payoutRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ProcessPayoutHandler(
        IPayoutRepository payoutRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _payoutRepository = payoutRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ProcessPayoutResponse> HandleAsync(long payoutId, CancellationToken cancellationToken = default)
    {
        var payout = await _payoutRepository.GetByIdAsync(payoutId, cancellationToken);
        if (payout == null)
        {
            return new ProcessPayoutResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Payout not found" }
            };
        }

        // TODO: Integrate with payment gateway to process payout
        var transactionId = $"TXN{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";

        payout.Status = "COMPLETED";
        payout.ProcessedAt = _dateTimeProvider.GetUtcNow();
        payout.TransactionReference = transactionId;
        payout.UpdatedAt = _dateTimeProvider.GetUtcNow();

        await _payoutRepository.UpdateAsync(payout, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new ProcessPayoutResponse
        {
            Success = true,
            Data = new ProcessPayoutResponseData
            {
                Id = payout.Id.ToString(),
                Status = "PROCESSED",
                TransactionId = transactionId,
                Message = "Payout processed successfully"
            }
        };
    }
}

