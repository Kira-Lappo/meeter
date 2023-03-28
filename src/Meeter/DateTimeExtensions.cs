using System;

namespace Meeter;

public static class DateTimeExtensions
{
    public static bool Between(
        this DateTime value,
        DateTime left,
        DateTime right)
    {
        return left <= value && value <= right;
    }
}
