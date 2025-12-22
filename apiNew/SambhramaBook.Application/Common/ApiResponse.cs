namespace SambhramaBook.Application.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public ErrorResponse? Error { get; set; }
    public PaginationInfo? Pagination { get; set; }
}

public class PaginationInfo
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public int TotalPages { get; set; }
}

public class ErrorResponse
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, string>? Details { get; set; }
}

