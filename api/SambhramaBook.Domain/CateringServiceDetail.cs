namespace SambhramaBook.Domain;

public sealed class CateringServiceDetail
{
    public Guid ServiceId { get; set; }
    public string? CuisineTypesJson { get; set; }
    public string? VegNonVegType { get; set; }
    public decimal MinimumOrderValue { get; set; }

    public Service Service { get; set; } = null!;
}

