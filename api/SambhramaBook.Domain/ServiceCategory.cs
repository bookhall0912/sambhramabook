namespace SambhramaBook.Domain;

public sealed class ServiceCategory
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string DisplayName { get; set; }
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public string? BackgroundImageUrl { get; set; }
    public string? ThemeColor { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
}
