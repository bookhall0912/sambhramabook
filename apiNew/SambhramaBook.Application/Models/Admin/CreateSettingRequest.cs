namespace SambhramaBook.Application.Models.Admin;

public class CreateSettingRequest
{
    public string Key { get; set; } = string.Empty;
    public object Value { get; set; } = string.Empty;
    public string? Description { get; set; }
}

