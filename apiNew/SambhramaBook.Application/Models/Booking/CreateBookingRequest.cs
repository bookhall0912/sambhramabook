namespace SambhramaBook.Application.Models.Booking;

public class CreateBookingRequest
{
    public string? HallId { get; set; } // Can be hall or service
    public string? ServiceId { get; set; } // For service bookings
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int NumberOfDays { get; set; }
    public int Guests { get; set; }
    public ContactInfoDto ContactInfo { get; set; } = new ContactInfoDto { Name = string.Empty };
    public string? SpecialRequests { get; set; }
    public string? EventType { get; set; }
    public string? EventName { get; set; }
    public long? ServicePackageId { get; set; } // For service bookings
    
    public long ListingId => !string.IsNullOrEmpty(HallId) && long.TryParse(HallId, out var hallId) 
        ? hallId 
        : (!string.IsNullOrEmpty(ServiceId) && long.TryParse(ServiceId, out var serviceId) ? serviceId : 0);
}

public class ContactInfoDto
{
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}

