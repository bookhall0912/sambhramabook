namespace SambhramaBook.Application.Models.Payment;

public class ProcessRefundRequest
{
    public decimal Amount { get; set; } // Optional, full refund if 0
    public string? Reason { get; set; }
}

