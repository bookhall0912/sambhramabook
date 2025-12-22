using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain;

public sealed class Payout
{
    public Guid Id { get; set; }
    public Guid VendorId { get; set; }
    public decimal Amount { get; set; }
    public PayoutStatus PayoutStatus { get; set; }
    public DateTime? PayoutDate { get; set; }

    public Vendor Vendor { get; set; } = null!;
}

