using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Repositories;

namespace SambhramaBook.Application.Handlers.Admin.Settings;

public interface IGetSettingHandler : IQueryHandler<string, GetSettingResponse?>;

public class GetSettingHandler : IGetSettingHandler
{
    private readonly IPlatformSettingRepository _platformSettingRepository;

    public GetSettingHandler(IPlatformSettingRepository platformSettingRepository)
    {
        _platformSettingRepository = platformSettingRepository;
    }

    public async Task<GetSettingResponse?> Handle(string key, CancellationToken ct)
    {
        var setting = await _platformSettingRepository.GetByKeyAsync(key, ct);
        if (setting == null)
        {
            return null;
        }

        return new GetSettingResponse
        {
            Success = true,
            Data = new SettingDto
            {
                Key = setting.SettingKey,
                Value = setting.SettingValue,
                Description = setting.Description
            }
        };
    }
}

