namespace SambhramaBook.Domain.Entities;

public class PlatformSetting
{
    public long Id { get; set; }
    public required string SettingKey { get; set; }
    public required string SettingValue { get; set; }
    public string SettingType { get; set; } = "STRING"; // STRING, NUMBER, BOOLEAN, JSON
    public string? Description { get; set; }
    public string? Category { get; set; } // PAYMENT, COMMISSION, NOTIFICATION, GENERAL
    public bool IsPublic { get; set; } = false; // Can be accessed via public API
    public long? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

