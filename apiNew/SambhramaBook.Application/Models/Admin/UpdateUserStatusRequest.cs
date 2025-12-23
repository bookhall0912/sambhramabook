namespace SambhramaBook.Application.Models.Admin;

public class UpdateUserStatusRequest
{
    public string Status { get; set; } = string.Empty; // ACTIVE | INACTIVE | SUSPENDED
}

