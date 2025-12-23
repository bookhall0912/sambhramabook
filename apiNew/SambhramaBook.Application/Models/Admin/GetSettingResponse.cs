namespace SambhramaBook.Application.Models.Admin;

public class GetSettingResponse
{
    public bool Success { get; set; }
    public SettingDto? Data { get; set; }
}

public class SettingDto
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
}

