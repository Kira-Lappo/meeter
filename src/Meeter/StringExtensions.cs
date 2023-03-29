﻿using System.Collections.Generic;

namespace Meeter;

public static class StringExtensions
{
    public static string JoinToString<T>(this IEnumerable<T> values, string separator)
    {
        return string.Join(separator, values);
    }
}
