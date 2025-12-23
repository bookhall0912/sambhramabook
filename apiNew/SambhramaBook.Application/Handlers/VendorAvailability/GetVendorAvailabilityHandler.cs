using SambhramaBook.Application.Models.VendorAvailability;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.VendorAvailability;

public interface IGetVendorAvailabilityHandler
{
    Task<GetVendorAvailabilityResponse> Handle(long userId, GetVendorAvailabilityRequest request, CancellationToken ct);
}

public class GetVendorAvailabilityHandler : IGetVendorAvailabilityHandler
{
    private readonly IListingRepository _listingRepository;
    private readonly IVendorQueries _vendorQueries;

    public GetVendorAvailabilityHandler(
        IListingRepository listingRepository,
        IVendorQueries vendorQueries)
    {
        _listingRepository = listingRepository;
        _vendorQueries = vendorQueries;
    }

    public async Task<GetVendorAvailabilityResponse> Handle(long userId, GetVendorAvailabilityRequest request, CancellationToken ct)
    {
        var listing = await _listingRepository.GetByIdAsync(request.ListingId, ct);
        if (listing == null || listing.VendorId != userId)
        {
            return new GetVendorAvailabilityResponse { Success = false };
        }

        var monthNumber = DateTime.ParseExact(request.Month, "MMMM", System.Globalization.CultureInfo.CurrentCulture).Month;
        var startDate = new DateOnly(request.Year, monthNumber, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var availability = (await _vendorQueries.GetVendorAvailabilityAsync(
            request.ListingId,
            startDate,
            endDate,
            ct)).ToList();

        var bookings = (await _vendorQueries.GetVendorBookingsForAvailabilityAsync(
            request.ListingId,
            startDate,
            endDate,
            ct)).ToList();

        var days = new List<AvailabilityDayDto>();
        var currentDate = startDate;

        while (currentDate <= endDate)
        {
            var dayAvailability = availability.FirstOrDefault(a => a.Date == currentDate);
            var isBooked = bookings.Any(b =>
                currentDate >= b.StartDate && currentDate <= b.EndDate);

            days.Add(new AvailabilityDayDto
            {
                Day = currentDate.Day,
                Status = isBooked ? "booked" :
                        dayAvailability?.Status == AvailabilityStatus.Blocked ? "blocked" :
                        dayAvailability?.Status == AvailabilityStatus.Maintenance ? "blocked" :
                        "available"
            });

            currentDate = currentDate.AddDays(1);
        }

        return new GetVendorAvailabilityResponse
        {
            Success = true,
            Data = new VendorAvailabilityData
            {
                ListingId = request.ListingId.ToString(),
                Month = request.Month,
                Year = request.Year,
                Days = days
            }
        };
    }
}

