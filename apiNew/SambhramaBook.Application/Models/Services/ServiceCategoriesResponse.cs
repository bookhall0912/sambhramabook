namespace SambhramaBook.Application.Models.Services;

public class ServiceCategoriesResponse
{
    public bool Success { get; set; }
    public ServiceCategoriesResponseData? Data { get; set; }
}

public class ServiceCategoriesResponseData
{
    public List<ServiceCategoryDto> Categories { get; set; } = [];
}

public class ServiceCategoryDto
{
    public string Code { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public string? BackgroundImageUrl { get; set; }
    public string? ThemeColor { get; set; }
    public int DisplayOrder { get; set; }
}

