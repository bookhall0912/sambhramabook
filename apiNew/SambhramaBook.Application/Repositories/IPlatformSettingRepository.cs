using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Repositories;

public interface IPlatformSettingRepository
{
    Task<PlatformSetting?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PlatformSetting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<IEnumerable<PlatformSetting>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PlatformSetting> CreateAsync(PlatformSetting setting, CancellationToken cancellationToken = default);
    Task<PlatformSetting> UpdateAsync(PlatformSetting setting, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

