using SambhramaBook.Application.Models.Booking;
using SambhramaBook.Application.Repositories;

namespace SambhramaBook.Application.Handlers.Booking;

public interface IGetBookingByIdHandler
{
    Task<BookingDetailDto?> Handle(long userId, long id, CancellationToken ct);
}

public class GetBookingByIdHandler : IGetBookingByIdHandler
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingByIdHandler(
        IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<BookingDetailDto?> Handle(long userId, long id, CancellationToken ct)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, ct);
        if (booking == null)
        {
            return null;
        }

        // Check authorization - user must be the customer or vendor
        if (booking.CustomerId != userId && booking.VendorId != userId)
        {
            return null; // Unauthorized
        }

        var primaryGuest = booking.Guests.FirstOrDefault(g => g.IsPrimaryContact);
        
        return new BookingDetailDto
        {
            Id = booking.Id,
            BookingId = booking.BookingReference,
            ReferenceId = booking.BookingReference,
            VenueName = booking.Listing.Title,
            VenueImage = booking.Listing.Images.FirstOrDefault(img => img.IsPrimary)?.ImageUrl,
            Location = $"{booking.Listing.City}, {booking.Listing.State}",
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Days = booking.DurationDays,
            GuestCount = booking.GuestCount,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status == Domain.Enums.BookingStatus.Cancelled ? "CANCELLED" :
                    booking.EndDate < DateOnly.FromDateTime(DateTime.UtcNow) ? "PAST" :
                    booking.Status == Domain.Enums.BookingStatus.Pending ? "PENDING_CONFIRMATION" : "UPCOMING",
            PaymentStatus = booking.PaymentStatus.ToString(),
            EventType = booking.EventType,
            ContactInfo = primaryGuest != null ? new ContactInfoDto
            {
                Name = primaryGuest.GuestName,
                Email = primaryGuest.GuestEmail,
                Phone = primaryGuest.GuestPhone
            } : null,
            SpecialRequests = booking.SpecialRequirements
        };
    }
}

