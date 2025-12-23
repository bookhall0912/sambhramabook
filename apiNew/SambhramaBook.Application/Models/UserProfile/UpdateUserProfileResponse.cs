using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.UserProfile;

public class UpdateUserProfileResponse
{
    public bool Success { get; set; }
    public UpdateUserProfileResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class UpdateUserProfileResponseData
{
    public string Id { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

