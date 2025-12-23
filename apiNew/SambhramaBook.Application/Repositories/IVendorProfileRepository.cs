using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Repositories;

public interface IVendorProfileRepository
{
    Task<VendorProfile?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<VendorProfile?> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<VendorProfile> CreateAsync(VendorProfile vendorProfile, CancellationToken cancellationToken = default);
    Task<VendorProfile> UpdateAsync(VendorProfile vendorProfile, CancellationToken cancellationToken = default);
}

