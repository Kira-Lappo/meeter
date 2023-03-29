namespace Meeter.Cli;

public static class StringExtensions
{
    public static string TrimWithEllipsis(this string value, int maxLength)
    {
        if (value.Length <= maxLength + 3)
        {
            return value;
        }

        return value[..maxLength] + "...";
    }
}
