using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.VendorBookings;

public class CreateVendorBookingResponse
{
    public bool Success { get; set; }
    public CreateVendorBookingResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class CreateVendorBookingResponseData
{
    public string BookingId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

