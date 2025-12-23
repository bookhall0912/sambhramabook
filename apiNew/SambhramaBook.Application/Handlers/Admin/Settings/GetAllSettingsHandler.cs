using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Repositories;

namespace SambhramaBook.Application.Handlers.Admin.Settings;

public interface IGetAllSettingsHandler : IQueryHandler<GetAllSettingsResponse>;

public class GetAllSettingsHandler : IGetAllSettingsHandler
{
    private readonly IPlatformSettingRepository _platformSettingRepository;

    public GetAllSettingsHandler(IPlatformSettingRepository platformSettingRepository)
    {
        _platformSettingRepository = platformSettingRepository;
    }

    public async Task<GetAllSettingsResponse> Handle(CancellationToken ct)
    {
        var settings = await _platformSettingRepository.GetAllAsync(ct);
        var settingsDict = settings.ToDictionary(s => s.SettingKey, s => s.SettingValue);

        return new GetAllSettingsResponse
        {
            Success = true,
            Data = new PlatformSettingsData
            {
                CommissionRate = decimal.TryParse(settingsDict.GetValueOrDefault("CommissionRate", "5"), out var commission) ? commission : 5,
                PlatformFee = decimal.TryParse(settingsDict.GetValueOrDefault("PlatformFee", "2"), out var fee) ? fee : 2,
                MinBookingAmount = decimal.TryParse(settingsDict.GetValueOrDefault("MinBookingAmount", "10000"), out var min) ? min : 10000,
                MaxBookingDays = int.TryParse(settingsDict.GetValueOrDefault("MaxBookingDays", "365"), out var max) ? max : 365,
                CancellationPolicy = settingsDict.GetValueOrDefault("CancellationPolicy", "Standard cancellation policy applies"),
                SupportEmail = settingsDict.GetValueOrDefault("SupportEmail", "support@sambhramabook.com"),
                SupportPhone = settingsDict.GetValueOrDefault("SupportPhone", "+91 98765 43210")
            }
        };
    }
}

