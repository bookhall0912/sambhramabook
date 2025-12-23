using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class UpdateBookingStatusResponse
{
    public bool Success { get; set; }
    public UpdateBookingStatusResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class UpdateBookingStatusResponseData
{
    public string Id { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

