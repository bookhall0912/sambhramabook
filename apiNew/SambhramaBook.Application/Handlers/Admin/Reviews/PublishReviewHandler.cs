using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Reviews;

public class PublishReviewHandler
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PublishReviewHandler(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<PublishReviewResponse> HandleAsync(long reviewId, PublishReviewRequest request, CancellationToken cancellationToken = default)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId, cancellationToken);
        if (review == null)
        {
            return new PublishReviewResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Review not found" }
            };
        }

        review.IsPublished = request.Published;
        review.UpdatedAt = _dateTimeProvider.GetUtcNow();

        await _reviewRepository.UpdateAsync(review, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new PublishReviewResponse
        {
            Success = true,
            Data = new PublishReviewResponseData
            {
                Id = review.Id.ToString(),
                Status = request.Published ? "published" : "pending",
                Message = "Review status updated"
            }
        };
    }
}

