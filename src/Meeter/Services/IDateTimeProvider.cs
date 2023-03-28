using System;

namespace Meeter.Services;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
