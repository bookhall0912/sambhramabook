using System.Text.Json;
using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Services;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Services;

public interface IGetServiceDetailsHandler : IQueryHandler<long, ServiceDetailDto?>;

public class GetServiceDetailsHandler : IGetServiceDetailsHandler
{
    private readonly IServiceQueries _serviceQueries;

    public GetServiceDetailsHandler(IServiceQueries serviceQueries)
    {
        _serviceQueries = serviceQueries;
    }

    public async Task<ServiceDetailDto?> Handle(long id, CancellationToken ct)
    {
        var listing = await _serviceQueries.GetServiceDetailsWithIncludesAsync(id, ct);

        if (listing == null || listing.ListingType == Domain.Enums.ListingType.Hall)
        {
            return null;
        }

        return new ServiceDetailDto
        {
            Id = listing.Id,
            ServiceType = (int)listing.ListingType,
            Title = listing.Title,
            Description = listing.ShortDescription,
            City = listing.City,
            Location = $"{listing.AddressLine1}, {listing.City}",
            Latitude = listing.Latitude,
            Longitude = listing.Longitude,
            Rating = listing.AverageRating,
            ReviewCount = listing.TotalReviews,
            Price = listing.BasePrice,
            ImageUrl = listing.Images.FirstOrDefault(img => img.IsPrimary)?.ImageUrl,
            Images = listing.Images.OrderBy(img => img.DisplayOrder).Select(img => img.ImageUrl).ToList(),
            VendorName = listing.Vendor.BusinessName,
            VendorImage = listing.Vendor.BusinessLogoUrl,
            VendorLocation = $"{listing.Vendor.City}, {listing.Vendor.State}",
            VendorRating = listing.Vendor.AverageRating,
            VendorReviewCount = listing.Vendor.TotalReviews,
            VendorExperience = 0, // YearsOfExperience doesn't exist on VendorProfile
            IsVerified = listing.Vendor.VerificationStatus == Domain.Enums.VerificationStatus.Approved,
            About = listing.Description,
            Specialities = [], // Specialities doesn't exist on Listing
            Packages = listing.ServicePackages.Select(p => new ServicePackageDto
            {
                Id = p.Id,
                Name = p.PackageName,
                Price = p.Price,
                PriceUnit = p.DurationHours.HasValue ? $"per_{p.DurationHours}_hours" : "per_event",
                Features = !string.IsNullOrEmpty(p.Includes) ? 
                    JsonSerializer.Deserialize<List<string>>(p.Includes) ?? [] : [],
                IsPopular = p.IsPopular
            }).ToList(),
            PortfolioImages = listing.Images.Where(img => !img.IsPrimary).Select(img => img.ImageUrl).ToList(),
            Reviews = listing.Reviews.Select(r => new ServiceReviewDto
            {
                Id = r.Id,
                Author = r.Customer.Name,
                Rating = r.Rating,
                Comment = r.Comment,
                Date = r.CreatedAt.ToString("yyyy-MM-dd"),
                Verified = r.IsVerifiedBooking
            }).ToList()
        };
    }
}

