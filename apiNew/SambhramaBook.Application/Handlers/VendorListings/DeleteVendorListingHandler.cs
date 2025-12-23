using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.VendorListings;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;

namespace SambhramaBook.Application.Handlers.VendorListings;

public class DeleteVendorListingHandler
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVendorListingHandler(
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteVendorListingResponse> HandleAsync(long userId, long listingId, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByVendorIdAndIdAsync(userId, listingId, cancellationToken);

        if (listing == null)
        {
            return new DeleteVendorListingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Listing not found" }
            };
        }

        await _listingRepository.DeleteAsync(listingId, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new DeleteVendorListingResponse
        {
            Success = true,
            Message = "Listing deleted successfully"
        };
    }
}

