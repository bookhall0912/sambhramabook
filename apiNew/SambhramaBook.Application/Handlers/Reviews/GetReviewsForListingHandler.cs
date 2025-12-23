using SambhramaBook.Application.Common;
using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Reviews;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Reviews;

public interface IGetReviewsForListingHandler : IQueryHandler<GetReviewsForListingRequest, GetReviewsForListingResponse>;

public class GetReviewsForListingHandler : IGetReviewsForListingHandler
{
    private readonly IReviewQueries _reviewQueries;

    public GetReviewsForListingHandler(IReviewQueries reviewQueries)
    {
        _reviewQueries = reviewQueries;
    }

    public async Task<GetReviewsForListingResponse> Handle(GetReviewsForListingRequest request, CancellationToken ct)
    {
        var (reviews, total) = await _reviewQueries.GetReviewsForListingAsync(
            request.ListingId,
            request.Page,
            request.PageSize,
            ct);

        var reviewDtos = reviews.Select(r => new ReviewDto
        {
            Id = r.Id.ToString(),
            Author = r.Customer.Name,
            Rating = r.Rating,
            Comment = r.Comment,
            Date = r.CreatedAt.ToString("yyyy-MM-dd"),
            Verified = r.IsVerifiedBooking
        }).ToList();

        return new GetReviewsForListingResponse
        {
            Success = true,
            Data = reviewDtos,
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

