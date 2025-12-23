using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class UpdateSettingResponse
{
    public bool Success { get; set; }
    public UpdateSettingResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class UpdateSettingResponseData
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

