using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Reviews;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;

namespace SambhramaBook.Application.Handlers.Reviews;

public class MarkReviewHelpfulHandler
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    public MarkReviewHelpfulHandler(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<MarkReviewHelpfulResponse> HandleAsync(long userId, long reviewId, MarkReviewHelpfulRequest request, CancellationToken cancellationToken = default)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId, cancellationToken);
        if (review == null)
        {
            return new MarkReviewHelpfulResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Review not found" }
            };
        }

        // TODO: Track helpful votes in a separate table
        // For now, just return success
        var helpfulCount = 0; // TODO: Get from database

        await _unitOfWork.SaveChanges(cancellationToken);

        return new MarkReviewHelpfulResponse
        {
            Success = true,
            Data = new MarkReviewHelpfulResponseData
            {
                Id = review.Id.ToString(),
                HelpfulCount = helpfulCount,
                Message = "Review marked as helpful"
            }
        };
    }
}

