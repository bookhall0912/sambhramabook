using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.VendorListings;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Handlers.VendorListings;

public class UploadListingImagesHandler
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UploadListingImagesHandler(
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<UploadListingImagesResponse> HandleAsync(long userId, long listingId, List<string> imageUrls, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdWithIncludesAsync(listingId, cancellationToken);
        
        if (listing == null || listing.VendorId != userId)

        if (listing == null)
        {
            return new UploadListingImagesResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Listing not found" }
            };
        }

        var existingImageCount = listing.Images.Count;
        var uploadedUrls = new List<string>();

        for (int i = 0; i < imageUrls.Count; i++)
        {
            var image = new ListingImage
            {
                ListingId = listingId,
                ImageUrl = imageUrls[i],
                IsPrimary = existingImageCount == 0 && i == 0,
                DisplayOrder = existingImageCount + i,
                CreatedAt = _dateTimeProvider.GetUtcNow()
            };
            listing.Images.Add(image);
            uploadedUrls.Add(imageUrls[i]);
        }

        await _listingRepository.UpdateAsync(listing, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new UploadListingImagesResponse
        {
            Success = true,
            Data = new UploadListingImagesResponseData
            {
                Images = uploadedUrls
            }
        };
    }
}

