namespace SambhramaBook.Application.Models.VendorSettings;

public class UpdateVendorSettingsRequest
{
    public NotificationSettingsDto? Notifications { get; set; }
    public BookingSettingsDto? BookingSettings { get; set; }
}

