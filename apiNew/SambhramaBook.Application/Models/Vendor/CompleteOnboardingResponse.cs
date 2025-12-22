namespace SambhramaBook.Application.Models.Vendor;

public class CompleteOnboardingResponse
{
    public bool Success { get; set; }
    public CompleteOnboardingResponseData? Data { get; set; }
}

public class CompleteOnboardingResponseData
{
    public long VendorId { get; set; }
    public bool OnboardingComplete { get; set; }
    public string Message { get; set; } = string.Empty;
}

