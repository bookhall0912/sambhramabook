namespace SambhramaBook.Application.Models.Admin;

public class GetAllSettingsResponse
{
    public bool Success { get; set; }
    public PlatformSettingsData? Data { get; set; }
}

public class PlatformSettingsData
{
    public decimal CommissionRate { get; set; }
    public decimal PlatformFee { get; set; }
    public decimal MinBookingAmount { get; set; }
    public int MaxBookingDays { get; set; }
    public string CancellationPolicy { get; set; } = string.Empty;
    public string SupportEmail { get; set; } = string.Empty;
    public string SupportPhone { get; set; } = string.Empty;
}

