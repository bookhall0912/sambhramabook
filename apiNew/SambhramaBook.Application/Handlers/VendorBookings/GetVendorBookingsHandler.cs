using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.VendorBookings;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.VendorBookings;

public interface IGetVendorBookingsHandler
{
    Task<GetVendorBookingsResponse> Handle(long userId, GetVendorBookingsRequest request, CancellationToken ct);
}

public class GetVendorBookingsHandler : IGetVendorBookingsHandler
{
    private readonly IBookingRepository _bookingRepository;

    public GetVendorBookingsHandler(
        IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<GetVendorBookingsResponse> Handle(long userId, GetVendorBookingsRequest request, CancellationToken ct)
    {
        var bookings = await _bookingRepository.GetByVendorIdAsync(userId, ct);
        
        // Filter by status
        if (!string.IsNullOrEmpty(request.Status) && request.Status.ToLower() != "all")
        {
            bookings = request.Status.ToLower() switch
            {
                "pending" => bookings.Where(b => b.Status == BookingStatus.Pending),
                "confirmed" => bookings.Where(b => b.Status == BookingStatus.Confirmed),
                _ => bookings
            };
        }

        var total = bookings.Count();
        var pagedBookings = bookings
            .OrderByDescending(b => b.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var bookingDtos = pagedBookings.Select(b => new VendorBookingDto
        {
            Id = b.Id,
            BookingId = b.BookingReference,
            CustomerName = b.Customer.Name,
            CustomerPhone = b.Customer.Mobile,
            VenueName = b.Listing.Title,
            Date = b.StartDate,
            Days = b.DurationDays,
            Guests = b.GuestCount,
            Amount = b.TotalAmount,
            Status = b.Status == BookingStatus.Confirmed ? "CONFIRMED" :
                    b.Status == BookingStatus.Pending ? "PENDING" :
                    "CANCELLED"
        }).ToList();

        return new GetVendorBookingsResponse
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

