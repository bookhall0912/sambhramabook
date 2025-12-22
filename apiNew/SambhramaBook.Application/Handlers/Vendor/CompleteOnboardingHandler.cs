using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Vendor;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Vendor;

public class CompleteOnboardingHandler
{
    private readonly IVendorProfileRepository _vendorProfileRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CompleteOnboardingHandler(
        IVendorProfileRepository vendorProfileRepository,
        IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _vendorProfileRepository = vendorProfileRepository;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<CompleteOnboardingResponse> HandleAsync(CompleteOnboardingRequest request, CancellationToken cancellationToken = default)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
        {
            return new CompleteOnboardingResponse
            {
                Success = false
            };
        }

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null || user.Role != UserRole.Vendor)
        {
            return new CompleteOnboardingResponse
            {
                Success = false
            };
        }

        var vendorProfile = await _vendorProfileRepository.GetByUserIdAsync(userId, cancellationToken);
        
        if (vendorProfile == null)
        {
            vendorProfile = new VendorProfile
            {
                UserId = userId,
                BusinessName = request.BusinessName,
                BusinessType = Enum.Parse<BusinessType>(request.BusinessType.Replace(" ", "")),
                BusinessEmail = request.BusinessEmail,
                BusinessPhone = request.BusinessPhone,
                AddressLine1 = request.Address,
                City = request.City,
                State = request.State,
                Pincode = request.Pincode,
                GstNumber = request.GstNumber,
                PanNumber = request.PanNumber,
                BankAccountNumber = request.BankAccountNumber,
                IfscCode = request.IfscCode,
                ProfileComplete = true,
                CreatedAt = _dateTimeProvider.GetUtcNow(),
                UpdatedAt = _dateTimeProvider.GetUtcNow()
            };

            vendorProfile = await _vendorProfileRepository.CreateAsync(vendorProfile, cancellationToken);
        }
        else
        {
            vendorProfile.BusinessName = request.BusinessName;
            vendorProfile.BusinessType = Enum.Parse<BusinessType>(request.BusinessType.Replace(" ", ""));
            vendorProfile.BusinessEmail = request.BusinessEmail;
            vendorProfile.BusinessPhone = request.BusinessPhone;
            vendorProfile.AddressLine1 = request.Address;
            vendorProfile.City = request.City;
            vendorProfile.State = request.State;
            vendorProfile.Pincode = request.Pincode;
            vendorProfile.GstNumber = request.GstNumber;
            vendorProfile.PanNumber = request.PanNumber;
            vendorProfile.BankAccountNumber = request.BankAccountNumber;
            vendorProfile.IfscCode = request.IfscCode;
            vendorProfile.ProfileComplete = true;
            vendorProfile.UpdatedAt = _dateTimeProvider.GetUtcNow();

            vendorProfile = await _vendorProfileRepository.UpdateAsync(vendorProfile, cancellationToken);
        }

        await _unitOfWork.SaveChanges(cancellationToken);

        return new CompleteOnboardingResponse
        {
            Success = true,
            Data = new CompleteOnboardingResponseData
            {
                VendorId = vendorProfile.Id,
                OnboardingComplete = true,
                Message = "Onboarding completed successfully"
            }
        };
    }
}

