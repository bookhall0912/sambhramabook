namespace SambhramaBook.Application.Common;

public interface IDateTimeProvider
{
    DateTime GetUtcNow();
    DateTime GetNow();
}

