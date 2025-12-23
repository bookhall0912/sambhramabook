using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Reviews;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Handlers.Reviews;

public class CreateReviewHandler
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    public CreateReviewHandler(
        IReviewRepository reviewRepository,
        IListingRepository listingRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _reviewRepository = reviewRepository;
        _listingRepository = listingRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<CreateReviewResponse> HandleAsync(long userId, CreateReviewRequest request, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing == null)
        {
            return new CreateReviewResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Listing not found" }
            };
        }

        // Check if user has a booking for this listing (for verified review)
        var bookings = await _bookingRepository.GetByCustomerIdAsync(userId, cancellationToken);
        var hasBooking = bookings.Any(b => b.ListingId == request.ListingId &&
                                          b.Status != Domain.Enums.BookingStatus.Cancelled);

        var review = new Review
        {
            ListingId = request.ListingId,
            CustomerId = userId,
            Rating = (int)request.Rating,
            Comment = request.Comment,
            IsVerifiedBooking = request.Verified && hasBooking,
            IsPublished = true,
            CreatedAt = _dateTimeProvider.GetUtcNow(),
            UpdatedAt = _dateTimeProvider.GetUtcNow()
        };

        review = await _reviewRepository.CreateAsync(review, cancellationToken);

        // Update listing rating
        var allReviews = await _reviewRepository.GetByListingIdAsync(request.ListingId, cancellationToken);
        listing.AverageRating = allReviews.Any() ? (decimal)allReviews.Average(r => (double)r.Rating) : (decimal)request.Rating;
        listing.TotalReviews = allReviews.Count();
        await _listingRepository.UpdateAsync(listing, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        return new CreateReviewResponse
        {
            Success = true,
            Data = new CreateReviewResponseData
            {
                Id = review.Id.ToString(),
                Message = "Review submitted successfully"
            }
        };
    }
}

