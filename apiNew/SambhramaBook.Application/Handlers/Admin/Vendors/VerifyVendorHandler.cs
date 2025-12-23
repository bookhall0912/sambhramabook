using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Vendors;

public class VerifyVendorHandler
{
    private readonly IVendorProfileRepository _vendorProfileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public VerifyVendorHandler(
        IVendorProfileRepository vendorProfileRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _vendorProfileRepository = vendorProfileRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<VerifyVendorResponse> HandleAsync(long vendorId, VerifyVendorRequest request, CancellationToken cancellationToken = default)
    {
        var vendorProfile = await _vendorProfileRepository.GetByIdAsync(vendorId, cancellationToken);
        if (vendorProfile == null)
        {
            return new VerifyVendorResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Vendor not found" }
            };
        }

        vendorProfile.VerificationStatus = Enum.Parse<VerificationStatus>(request.VerificationStatus, ignoreCase: true);
        vendorProfile.UpdatedAt = _dateTimeProvider.GetUtcNow();

        await _vendorProfileRepository.UpdateAsync(vendorProfile, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new VerifyVendorResponse
        {
            Success = true,
            Data = new VerifyVendorResponseData
            {
                Id = vendorProfile.Id.ToString(),
                VerificationStatus = request.VerificationStatus,
                Message = "Vendor verification updated"
            }
        };
    }
}

