namespace SambhramaBook.Application.Common;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetUtcNow() => DateTime.UtcNow;
    public DateTime GetNow() => DateTime.Now;
}

