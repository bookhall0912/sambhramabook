using SambhramaBook.Application.Common;
using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Admin.Reviews;

public interface IGetAllReviewsHandler : IQueryHandler<GetAllReviewsRequest, GetAllReviewsResponse>;

public class GetAllReviewsHandler : IGetAllReviewsHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetAllReviewsHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetAllReviewsResponse> Handle(GetAllReviewsRequest request, CancellationToken ct)
    {
        var (reviews, total) = await _adminQueries.GetAllReviewsAsync(
            request.Page,
            request.PageSize,
            ct);

        var reviewDtos = reviews.Select(r => new AdminReviewDto
        {
            Id = r.Id.ToString(),
            Author = r.Customer?.Name ?? "",
            ListingId = r.ListingId.ToString(),
            ListingName = r.Listing?.Title ?? "",
            Rating = r.Rating,
            Comment = r.Comment,
            Date = r.CreatedAt.ToString("yyyy-MM-dd"),
            Status = r.IsPublished ? "published" : "pending",
            Verified = r.IsVerifiedBooking
        }).ToList();

        return new GetAllReviewsResponse
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

