using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Vendors;

public class UpdateVendorStatusHandler
{
    private readonly IVendorProfileRepository _vendorProfileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateVendorStatusHandler(
        IVendorProfileRepository vendorProfileRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _vendorProfileRepository = vendorProfileRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<UpdateVendorStatusResponse> HandleAsync(long vendorId, UpdateVendorStatusRequest request, CancellationToken cancellationToken = default)
    {
        var vendorProfile = await _vendorProfileRepository.GetByIdAsync(vendorId, cancellationToken);
        if (vendorProfile == null)
        {
            return new UpdateVendorStatusResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Vendor not found" }
            };
        }

        // Update user's IsActive status instead
        if (vendorProfile.User != null)
        {
            vendorProfile.User.IsActive = request.Status.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase);
            vendorProfile.User.UpdatedAt = _dateTimeProvider.GetUtcNow();
        }
        vendorProfile.UpdatedAt = _dateTimeProvider.GetUtcNow();

        await _vendorProfileRepository.UpdateAsync(vendorProfile, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new UpdateVendorStatusResponse
        {
            Success = true,
            Data = new UpdateVendorStatusResponseData
            {
                Id = vendorProfile.Id.ToString(),
                Status = request.Status,
                Message = "Vendor status updated"
            }
        };
    }
}

