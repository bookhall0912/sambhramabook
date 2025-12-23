namespace SambhramaBook.Application.Models.VendorSettings;

public class VendorSettingsResponse
{
    public bool Success { get; set; }
    public VendorSettingsData? Data { get; set; }
}

public class VendorSettingsData
{
    public NotificationSettingsDto Notifications { get; set; } = new();
    public BookingSettingsDto BookingSettings { get; set; } = new();
}

public class NotificationSettingsDto
{
    public bool Email { get; set; }
    public bool Sms { get; set; }
    public bool Push { get; set; }
}

public class BookingSettingsDto
{
    public bool AutoApprove { get; set; }
    public bool RequireDeposit { get; set; }
}

