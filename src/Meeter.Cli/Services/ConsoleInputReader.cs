namespace Meeter.Cli.Services;

public class ConsoleInputReader
{
    public void ReadDateTimeAndExecute(Action<DateTime> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        Console.WriteLine("Введите дату:");
        if (TryReadDateTimeWithNow(out var dateTime))
        {
            action(dateTime);
        }
        else
        {
            Console.WriteLine("Неподдерживаемый формат даты");
        }
    }

    public bool TryReadDateTime(out DateTime dateTime)
    {
        var input = Console.ReadLine();
        if (DateTime.TryParse(input, out dateTime))
        {
            return true;
        }

        return false;
    }

    public bool TryReadDateTimeWithNow(out DateTime dateTime)
    {
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input)
            || string.Equals(input, "*", StringComparison.Ordinal)
            || string.Equals(input, "now", StringComparison.OrdinalIgnoreCase)
        )
        {
            dateTime = DateTime.UtcNow;
            return true;
        }

        if (DateTime.TryParse(input, out dateTime))
        {
            return true;
        }

        return false;
    }
}
