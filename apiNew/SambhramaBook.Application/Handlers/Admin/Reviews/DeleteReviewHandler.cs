using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;

namespace SambhramaBook.Application.Handlers.Admin.Reviews;

public class DeleteReviewHandler
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteReviewHandler(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteReviewResponse> HandleAsync(long reviewId, CancellationToken cancellationToken = default)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId, cancellationToken);
        if (review == null)
        {
            return new DeleteReviewResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Review not found" }
            };
        }

        await _reviewRepository.DeleteAsync(reviewId, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new DeleteReviewResponse
        {
            Success = true,
            Message = "Review deleted successfully"
        };
    }
}

