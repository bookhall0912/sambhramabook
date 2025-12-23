using SambhramaBook.Application.Models.VendorSettings;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.VendorSettings;

public interface IGetVendorSettingsHandler
{
    Task<VendorSettingsResponse> Handle(long userId, CancellationToken ct);
}

public class GetVendorSettingsHandler : IGetVendorSettingsHandler
{
    private readonly IVendorQueries _vendorQueries;

    public GetVendorSettingsHandler(IVendorQueries vendorQueries)
    {
        _vendorQueries = vendorQueries;
    }

    public async Task<VendorSettingsResponse> Handle(long userId, CancellationToken ct)
    {
        var vendorProfile = await _vendorQueries.GetVendorSettingsAsync(userId, ct);

        // Default settings if not found
        return new VendorSettingsResponse
        {
            Success = true,
            Data = new VendorSettingsData
            {
                Notifications = new NotificationSettingsDto
                {
                    Email = true,
                    Sms = false,
                    Push = true
                },
                BookingSettings = new BookingSettingsDto
                {
                    AutoApprove = false,
                    RequireDeposit = true
                }
            }
        };
    }
}

