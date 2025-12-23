namespace SambhramaBook.Application.Models.Admin;

public class GetPendingListingsForApprovalRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

