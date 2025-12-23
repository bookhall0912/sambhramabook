using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Services;

namespace SambhramaBook.Application.Handlers.LandingPage;

public interface IGetServiceCategoriesHandler : IQueryHandler<ServiceCategoriesResponse>;

public class GetServiceCategoriesHandler : IGetServiceCategoriesHandler
{
    public Task<ServiceCategoriesResponse> Handle(CancellationToken ct)
    {
        // Static service categories - can be moved to database later
        var categories = new List<ServiceCategoryDto>
        {
            new() { Code = "photography", DisplayName = "Photography", Description = "Professional wedding photographers", DisplayOrder = 1 },
            new() { Code = "catering", DisplayName = "Catering", Description = "Delicious wedding catering services", DisplayOrder = 2 },
            new() { Code = "decoration", DisplayName = "Decoration", Description = "Beautiful wedding decorations", DisplayOrder = 3 },
            new() { Code = "music", DisplayName = "Music & DJ", Description = "Wedding music and DJ services", DisplayOrder = 4 },
            new() { Code = "makeup", DisplayName = "Makeup & Beauty", Description = "Professional makeup artists", DisplayOrder = 5 },
            new() { Code = "videography", DisplayName = "Videography", Description = "Wedding video services", DisplayOrder = 6 }
        };

        return Task.FromResult(new ServiceCategoriesResponse
        {
            Success = true,
            Data = new ServiceCategoriesResponseData
            {
                Categories = categories
            }
        });
    }
}

