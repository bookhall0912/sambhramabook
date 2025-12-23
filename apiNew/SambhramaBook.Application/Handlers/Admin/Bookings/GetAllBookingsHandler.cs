using SambhramaBook.Application.Common;
using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Bookings;

public interface IGetAllBookingsHandler : IQueryHandler<GetAllBookingsRequest, GetAllBookingsResponse>;

public class GetAllBookingsHandler : IGetAllBookingsHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetAllBookingsHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetAllBookingsResponse> Handle(GetAllBookingsRequest request, CancellationToken ct)
    {
        var dateFrom = request.StartDate?.ToString("yyyy-MM-dd");
        var dateTo = request.EndDate?.ToString("yyyy-MM-dd");
        
        var (bookings, total) = await _adminQueries.GetAllBookingsAsync(
            request.Status,
            dateFrom,
            dateTo,
            request.Page,
            request.PageSize,
            ct);

        var bookingDtos = bookings.Select(b => new AdminBookingDto
        {
            Id = b.Id.ToString(),
            BookingId = b.BookingReference,
            CustomerName = b.Customer?.Name ?? "",
            VenueName = b.Listing?.Title ?? "",
            Date = b.StartDate.ToString("yyyy-MM-dd"),
            Amount = b.TotalAmount,
            Status = b.Status == BookingStatus.Confirmed ? "CONFIRMED" :
                    b.Status == BookingStatus.Pending ? "PENDING" :
                    b.Status == BookingStatus.Cancelled ? "CANCELLED" : "COMPLETED"
        }).ToList();

        return new GetAllBookingsResponse
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

