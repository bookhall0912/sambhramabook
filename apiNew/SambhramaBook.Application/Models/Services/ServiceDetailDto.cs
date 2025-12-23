namespace SambhramaBook.Application.Models.Services;

public class ServiceDetailDto : ServiceListItemDto
{
    public string VendorName { get; set; } = string.Empty;
    public string? VendorImage { get; set; }
    public string VendorLocation { get; set; } = string.Empty;
    public decimal VendorRating { get; set; }
    public int VendorReviewCount { get; set; }
    public int VendorExperience { get; set; }
    public bool IsVerified { get; set; }
    public string? About { get; set; }
    public List<string> Specialities { get; set; } = [];
    public List<ServicePackageDto> Packages { get; set; } = [];
    public List<string> PortfolioImages { get; set; } = [];
    public List<ServiceReviewDto> Reviews { get; set; } = [];
}

public class ServicePackageDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string PriceUnit { get; set; } = string.Empty;
    public List<string> Features { get; set; } = [];
    public bool IsPopular { get; set; }
}

public class ServiceReviewDto
{
    public long Id { get; set; }
    public string Author { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public string? Comment { get; set; }
    public string Date { get; set; } = string.Empty;
    public bool Verified { get; set; }
}

