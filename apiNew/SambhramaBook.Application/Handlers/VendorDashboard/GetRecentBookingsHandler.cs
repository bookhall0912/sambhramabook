using SambhramaBook.Application.Models.VendorDashboard;
using SambhramaBook.Application.Repositories;

namespace SambhramaBook.Application.Handlers.VendorDashboard;

public interface IGetRecentBookingsHandler
{
    Task<GetRecentBookingsResponse> Handle(long userId, int limit, CancellationToken ct);
}

public class GetRecentBookingsHandler : IGetRecentBookingsHandler
{
    private readonly IBookingRepository _bookingRepository;

    public GetRecentBookingsHandler(
        IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<GetRecentBookingsResponse> Handle(long userId, int limit, CancellationToken ct)
    {
        var bookings = await _bookingRepository.GetByVendorIdAsync(userId, ct);
        var recentBookings = bookings
            .OrderByDescending(b => b.CreatedAt)
            .Take(limit)
            .Select(b => new RecentBookingDto
            {
                Id = b.Id.ToString(),
                BookingId = b.BookingReference,
                CustomerName = b.Customer.Name,
                Date = b.StartDate.ToString("yyyy-MM-dd"),
                Status = b.Status == Domain.Enums.BookingStatus.Confirmed ? "CONFIRMED" : "PENDING"
            })
            .ToList();

        return new GetRecentBookingsResponse
        {
            Success = true,
            Data = recentBookings
        };
    }
}

