using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.Repository;

public class PlatformSettingRepository : IPlatformSettingRepository
{
    private readonly SambhramaBookDbContext _context;

    public PlatformSettingRepository(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<PlatformSetting?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.PlatformSettings
            .FirstOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }

    public async Task<PlatformSetting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _context.PlatformSettings
            .FirstOrDefaultAsync(ps => ps.SettingKey == key, cancellationToken);
    }

    public async Task<IEnumerable<PlatformSetting>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PlatformSettings
            .OrderBy(ps => ps.Category)
            .ThenBy(ps => ps.SettingKey)
            .ToListAsync(cancellationToken);
    }

    public async Task<PlatformSetting> CreateAsync(PlatformSetting setting, CancellationToken cancellationToken = default)
    {
        setting.UpdatedAt = DateTime.UtcNow;
        _context.PlatformSettings.Add(setting);
        await _context.SaveChangesAsync(cancellationToken);
        return setting;
    }

    public async Task<PlatformSetting> UpdateAsync(PlatformSetting setting, CancellationToken cancellationToken = default)
    {
        setting.UpdatedAt = DateTime.UtcNow;
        _context.PlatformSettings.Update(setting);
        await _context.SaveChangesAsync(cancellationToken);
        return setting;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var setting = await _context.PlatformSettings.FindAsync([id], cancellationToken);
        if (setting != null)
        {
            _context.PlatformSettings.Remove(setting);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

