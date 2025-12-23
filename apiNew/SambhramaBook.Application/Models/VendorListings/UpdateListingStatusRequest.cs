namespace SambhramaBook.Application.Models.VendorListings;

public class UpdateListingStatusRequest
{
    public string Status { get; set; } = string.Empty; // DRAFT | ACTIVE | INACTIVE
}

