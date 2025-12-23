using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Booking;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.UserBookings;

public interface IGetUserBookingsHandler
{
    Task<GetUserBookingsResponse> Handle(long userId, GetUserBookingsRequest request, CancellationToken ct);
}

public class GetUserBookingsHandler : IGetUserBookingsHandler
{
    private readonly IBookingRepository _bookingRepository;

    public GetUserBookingsHandler(
        IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<GetUserBookingsResponse> Handle(long userId, GetUserBookingsRequest request, CancellationToken ct)
    {
        var bookings = await _bookingRepository.GetByCustomerIdAsync(userId, ct);
        
        // Filter by status
        if (!string.IsNullOrEmpty(request.Status))
        {
            bookings = request.Status.ToUpper() switch
            {
                "UPCOMING" => bookings.Where(b => b.Status != BookingStatus.Cancelled && b.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow)),
                "PAST" => bookings.Where(b => b.EndDate < DateOnly.FromDateTime(DateTime.UtcNow)),
                "CANCELLED" => bookings.Where(b => b.Status == BookingStatus.Cancelled),
                _ => bookings
            };
        }

        var total = bookings.Count();
        var pagedBookings = bookings
            .OrderByDescending(b => b.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var bookingDtos = pagedBookings.Select(b => new UserBookingDto
        {
            Id = b.Id,
            ReferenceId = b.BookingReference,
            VenueName = b.Listing.Title,
            VenueImage = b.Listing.Images.FirstOrDefault(img => img.IsPrimary)?.ImageUrl,
            Location = $"{b.Listing.City}, {b.Listing.State}",
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            Days = b.DurationDays,
            GuestCount = b.GuestCount,
            TotalAmount = b.TotalAmount,
            Status = b.Status == BookingStatus.Cancelled ? "CANCELLED" :
                    b.EndDate < DateOnly.FromDateTime(DateTime.UtcNow) ? "PAST" :
                    b.Status == BookingStatus.Pending ? "PENDING_CONFIRMATION" : "UPCOMING",
            PaymentStatus = b.PaymentStatus.ToString(),
            EventType = b.EventType
        }).ToList();

        return new GetUserBookingsResponse
        {
            Success = true,
            Data = bookingDtos,
            Pagination = new PaginationInfo
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Total = total,
                TotalPages = (int)Math.Ceiling(total / (double)request.PageSize)
            }
        };
    }
}

