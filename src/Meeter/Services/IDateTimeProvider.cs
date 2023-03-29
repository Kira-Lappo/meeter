namespace Meeter.Services;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }

    DateTime Now { get; }
}
