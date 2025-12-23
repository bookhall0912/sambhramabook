using SambhramaBook.Application.Models.Vendor;
using SambhramaBook.Application.Repositories;

namespace SambhramaBook.Application.Handlers.Vendor;

public class GetOnboardingStatusHandler
{
    private readonly IVendorProfileRepository _vendorProfileRepository;

    public GetOnboardingStatusHandler(
        IVendorProfileRepository vendorProfileRepository)
    {
        _vendorProfileRepository = vendorProfileRepository;
    }

    public async Task<GetOnboardingStatusResponse> HandleAsync(long userId, CancellationToken cancellationToken = default)
    {
        var vendorProfile = await _vendorProfileRepository.GetByUserIdAsync(userId, cancellationToken);

        var completedSteps = new List<string>();
        var pendingSteps = new List<string>();

        if (vendorProfile != null)
        {
            if (!string.IsNullOrEmpty(vendorProfile.BusinessName) && 
                !string.IsNullOrEmpty(vendorProfile.BusinessEmail) &&
                !string.IsNullOrEmpty(vendorProfile.BusinessPhone))
            {
                completedSteps.Add("business_info");
            }
            else
            {
                pendingSteps.Add("business_info");
            }

            if (!string.IsNullOrEmpty(vendorProfile.AddressLine1) &&
                !string.IsNullOrEmpty(vendorProfile.City) &&
                !string.IsNullOrEmpty(vendorProfile.State) &&
                !string.IsNullOrEmpty(vendorProfile.Pincode))
            {
                completedSteps.Add("location");
            }
            else
            {
                pendingSteps.Add("location");
            }

            if (!string.IsNullOrEmpty(vendorProfile.GstNumber) ||
                !string.IsNullOrEmpty(vendorProfile.PanNumber) ||
                !string.IsNullOrEmpty(vendorProfile.BankAccountNumber))
            {
                completedSteps.Add("additional_details");
            }
            else
            {
                pendingSteps.Add("additional_details");
            }
        }
        else
        {
            pendingSteps.AddRange(["business_info", "location", "additional_details"]);
        }

        return new GetOnboardingStatusResponse
        {
            Success = true,
            Data = new GetOnboardingStatusResponseData
            {
                OnboardingComplete = vendorProfile?.ProfileComplete ?? false,
                CompletedSteps = completedSteps,
                PendingSteps = pendingSteps
            }
        };
    }
}

