namespace SambhramaBook.Application.Models.Admin;

public class UpdatePayoutStatusRequest
{
    public string Status { get; set; } = string.Empty; // PENDING | PROCESSED | FAILED
    public string? TransactionId { get; set; }
    public string? Notes { get; set; }
}

