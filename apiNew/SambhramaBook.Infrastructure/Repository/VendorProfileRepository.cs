using Microsoft.EntityFrameworkCore;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.Repository;

public class VendorProfileRepository : IVendorProfileRepository
{
    private readonly SambhramaBookDbContext _context;

    public VendorProfileRepository(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<VendorProfile?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.VendorProfiles
            .Include(vp => vp.User)
            .FirstOrDefaultAsync(vp => vp.Id == id, cancellationToken);
    }

    public async Task<VendorProfile?> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.VendorProfiles
            .Include(vp => vp.User)
            .FirstOrDefaultAsync(vp => vp.UserId == userId, cancellationToken);
    }

    public async Task<VendorProfile> CreateAsync(VendorProfile vendorProfile, CancellationToken cancellationToken = default)
    {
        _context.VendorProfiles.Add(vendorProfile);
        await _context.SaveChangesAsync(cancellationToken);
        return vendorProfile;
    }

    public async Task<VendorProfile> UpdateAsync(VendorProfile vendorProfile, CancellationToken cancellationToken = default)
    {
        vendorProfile.UpdatedAt = DateTime.UtcNow;
        _context.VendorProfiles.Update(vendorProfile);
        await _context.SaveChangesAsync(cancellationToken);
        return vendorProfile;
    }
}

