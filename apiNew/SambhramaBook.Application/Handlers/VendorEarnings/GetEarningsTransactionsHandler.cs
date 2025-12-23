using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.VendorEarnings;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.VendorEarnings;

public interface IGetEarningsTransactionsHandler
{
    Task<GetEarningsTransactionsResponse> Handle(long userId, GetEarningsTransactionsRequest request, CancellationToken ct);
}

public class GetEarningsTransactionsHandler : IGetEarningsTransactionsHandler
{
    private readonly IVendorQueries _vendorQueries;

    public GetEarningsTransactionsHandler(IVendorQueries vendorQueries)
    {
        _vendorQueries = vendorQueries;
    }

    public async Task<GetEarningsTransactionsResponse> Handle(long userId, GetEarningsTransactionsRequest request, CancellationToken ct)
    {
        var (transactions, total) = await _vendorQueries.GetEarningsTransactionsAsync(
            userId,
            request.Page,
            request.PageSize,
            ct);

        var transactionDtos = transactions
            .Where(t => t.Booking != null)
            .Select(t => new EarningsTransactionDto
            {
                Id = t.Booking!.Id.ToString(),
                BookingId = t.Booking.BookingReference,
                Amount = t.Booking.TotalAmount,
                Commission = t.Booking.PlatformFee,
                NetAmount = t.Booking.TotalAmount - t.Booking.PlatformFee,
                Date = t.Booking.StartDate.ToString("yyyy-MM-dd"),
                Status = "completed"
            }).ToList();

        return new GetEarningsTransactionsResponse
        {
            Success = true,
            Data = transactionDtos,
            Pagination = new PaginationInfo
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Total = total,
                TotalPages = (int)Math.Ceiling(total / (double)request.PageSize)
            }
        };
    }
}

