namespace SambhramaBook.Domain;

public sealed class Commission
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public decimal PlatformFee { get; set; }
    public decimal VendorEarnings { get; set; }

    public Booking Booking { get; set; } = null!;
}

