using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class CreateSettingResponse
{
    public bool Success { get; set; }
    public CreateSettingResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class CreateSettingResponseData
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

