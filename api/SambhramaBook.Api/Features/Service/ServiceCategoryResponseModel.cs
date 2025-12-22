using SambhramaBook.Domain;

namespace SambhramaBook.Api.Features.Service;

public sealed class ServiceCategoryResponseModel
{
    public ServiceCategoryResponseModel(ServiceCategory category)
    {
        Code = category.Code;
        DisplayName = category.DisplayName;
        Description = category.Description;
        IconUrl = category.IconUrl;
        BackgroundImageUrl = category.BackgroundImageUrl;
        ThemeColor = category.ThemeColor;
        DisplayOrder = category.DisplayOrder;
    }

    public string Code { get; }
    public string DisplayName { get; }
    public string? Description { get; }
    public string? IconUrl { get; }
    public string? BackgroundImageUrl { get; }
    public string? ThemeColor { get; }
    public int DisplayOrder { get; }
}
