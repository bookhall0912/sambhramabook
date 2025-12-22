namespace SambhramaBook.Application.Models.Booking;

public class CreateBookingRequest
{
    public long ListingId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int NumberOfDays { get; set; }
    public int Guests { get; set; }
    public ContactInfoDto ContactInfo { get; set; } = new ContactInfoDto { Name = string.Empty };
    public string? SpecialRequests { get; set; }
    public string? EventType { get; set; }
    public string? EventName { get; set; }
    public long? ServicePackageId { get; set; } // For service bookings
}

public class ContactInfoDto
{
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}

