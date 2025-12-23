using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Reviews;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Reviews;

public class AddVendorResponseHandler
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AddVendorResponseHandler(
        IReviewRepository reviewRepository,
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _reviewRepository = reviewRepository;
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<AddVendorResponseResponse> HandleAsync(long userId, long reviewId, AddVendorResponseRequest request, CancellationToken cancellationToken = default)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId, cancellationToken);
        if (review == null)
        {
            return new AddVendorResponseResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Review not found" }
            };
        }

        var listing = await _listingRepository.GetByIdAsync(review.ListingId, cancellationToken);
        if (listing == null || listing.VendorId != userId)
        {
            return new AddVendorResponseResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "FORBIDDEN", Message = "Not authorized" }
            };
        }

        review.VendorResponse = request.Response;
        review.VendorRespondedAt = _dateTimeProvider.GetUtcNow();
        review.UpdatedAt = _dateTimeProvider.GetUtcNow();

        await _reviewRepository.UpdateAsync(review, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new AddVendorResponseResponse
        {
            Success = true,
            Data = new AddVendorResponseResponseData
            {
                Id = review.Id.ToString(),
                VendorResponse = review.VendorResponse,
                Message = "Response added successfully"
            }
        };
    }
}

