using System;

namespace Meeter;

public static class DateTimeRangeHelper
{
    public static bool HasOverlap(
        DateTime firstStart,
        DateTime firstEnd,
        DateTime secondStart,
        DateTime secondEnd)
    {
        if (firstStart <= secondStart)
        {
            return secondStart < firstEnd;
        }

        return HasOverlap(secondStart, secondEnd, firstStart, firstEnd);
    }
}
