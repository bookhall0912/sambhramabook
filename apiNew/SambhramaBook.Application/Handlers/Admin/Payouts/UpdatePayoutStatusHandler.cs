using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Payouts;

public class UpdatePayoutStatusHandler
{
    private readonly IPayoutRepository _payoutRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdatePayoutStatusHandler(
        IPayoutRepository payoutRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _payoutRepository = payoutRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<UpdatePayoutStatusResponse> HandleAsync(long payoutId, UpdatePayoutStatusRequest request, CancellationToken cancellationToken = default)
    {
        var payout = await _payoutRepository.GetByIdAsync(payoutId, cancellationToken);
        if (payout == null)
        {
            return new UpdatePayoutStatusResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Payout not found" }
            };
        }

        payout.Status = request.Status.ToUpperInvariant();
        if (!string.IsNullOrEmpty(request.TransactionId))
        {
            payout.TransactionReference = request.TransactionId;
        }
        if (payout.Status == "COMPLETED" && payout.ProcessedAt == null)
        {
            payout.ProcessedAt = _dateTimeProvider.GetUtcNow();
        }
        payout.UpdatedAt = _dateTimeProvider.GetUtcNow();

        await _payoutRepository.UpdateAsync(payout, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new UpdatePayoutStatusResponse
        {
            Success = true,
            Data = new UpdatePayoutStatusResponseData
            {
                Id = payout.Id.ToString(),
                Status = request.Status,
                Message = "Payout status updated"
            }
        };
    }
}

