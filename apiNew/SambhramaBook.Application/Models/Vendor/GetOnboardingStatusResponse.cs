namespace SambhramaBook.Application.Models.Vendor;

public class GetOnboardingStatusResponse
{
    public bool Success { get; set; }
    public GetOnboardingStatusResponseData? Data { get; set; }
}

public class GetOnboardingStatusResponseData
{
    public bool OnboardingComplete { get; set; }
    public List<string> CompletedSteps { get; set; } = [];
    public List<string> PendingSteps { get; set; } = [];
}

