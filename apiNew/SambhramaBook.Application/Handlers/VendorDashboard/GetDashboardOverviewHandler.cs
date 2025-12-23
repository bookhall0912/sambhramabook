using SambhramaBook.Application.Models.VendorDashboard;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.VendorDashboard;

public interface IGetDashboardOverviewHandler
{
    Task<DashboardOverviewResponse> Handle(long userId, CancellationToken ct);
}

public class GetDashboardOverviewHandler : IGetDashboardOverviewHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IListingRepository _listingRepository;

    public GetDashboardOverviewHandler(
        IBookingRepository bookingRepository,
        IListingRepository listingRepository)
    {
        _bookingRepository = bookingRepository;
        _listingRepository = listingRepository;
    }

    public async Task<DashboardOverviewResponse> Handle(long userId, CancellationToken ct)
    {
        var bookings = await _bookingRepository.GetByVendorIdAsync(userId, ct);
        var listings = await _listingRepository.GetByVendorIdAsync(userId, ct);

        var totalEarnings = bookings
            .Where(b => b.PaymentStatus == PaymentStatus.Paid)
            .Sum(b => b.TotalAmount);

        var upcomingEvents = bookings
            .Count(b => b.Status != BookingStatus.Cancelled && b.StartDate >= DateOnly.FromDateTime(DateTime.UtcNow));

        var pendingBookings = bookings.Count(b => b.Status == BookingStatus.Pending);

        var recentBookings = bookings
            .OrderByDescending(b => b.CreatedAt)
            .Take(5)
            .Select(b => new RecentBookingDto
            {
                Id = b.Id.ToString(),
                BookingId = b.BookingReference,
                CustomerName = b.Customer.Name,
                Date = b.StartDate.ToString("yyyy-MM-dd"),
                Status = b.Status == BookingStatus.Confirmed ? "CONFIRMED" : "PENDING"
            })
            .ToList();

        var listingDtos = listings
            .Take(5)
            .Select(l => new ListingSummaryDto
            {
                Id = l.Id.ToString(),
                Name = l.Title,
                Image = l.Images.FirstOrDefault(img => img.IsPrimary)?.ImageUrl,
                Location = l.City,
                Status = l.Status == ListingStatus.Approved ? "ACTIVE" :
                        l.Status == ListingStatus.Draft ? "DRAFT" : "INACTIVE"
            })
            .ToList();

        var nextBooking = bookings
            .Where(b => b.Status != BookingStatus.Cancelled && b.StartDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .OrderBy(b => b.StartDate)
            .FirstOrDefault();

        var nextEvent = nextBooking != null
            ? $"{nextBooking.EventName ?? "Event"} ({nextBooking.StartDate:dd MMM})"
            : "No upcoming events";

        return new DashboardOverviewResponse
        {
            Success = true,
            Data = new DashboardOverviewData
            {
                TotalEarnings = totalEarnings,
                UpcomingEvents = upcomingEvents,
                ProfileViews = 0, // TODO: Implement profile views tracking
                EarningsChange = 0, // TODO: Calculate from previous period
                ProfileViewsChange = 0, // TODO: Calculate from previous period
                NextEvent = nextEvent,
                RecentBookings = recentBookings,
                Listings = listingDtos,
                PendingBookingsCount = pendingBookings
            }
        };
    }
}

