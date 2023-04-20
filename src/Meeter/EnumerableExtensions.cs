namespace Meeter;

public static class EnumerableExtensions
{
    public static IEnumerable<(T item, int index)> Indexed<T>(this IEnumerable<T> source, int startIndex = 0)
    {
        return source.Select((item, index) => (item, index + startIndex));
    }

    public static string JoinToString<T>(this IEnumerable<T> values, string separator)
    {
        ArgumentNullException.ThrowIfNull(values);

        return string.Join(separator, values);
    }
}
