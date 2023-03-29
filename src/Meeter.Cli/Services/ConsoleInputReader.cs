namespace Meeter.Cli.Services;

public class ConsoleInputReader
{
    public TimeSpan ReadTimeSpanUntilValid(string prompt = default, TimeSpan? defaultValue = default)
    {
        while (true)
        {
            if (TryReadTimeSpan(out var value, prompt, defaultValue))
            {
                return value;
            }
        }
    }

    public bool TryReadTimeSpan(out TimeSpan timeSpan, string prompt = default, TimeSpan? defaultValue = default)
    {
        prompt = CreatePrompt(prompt, defaultValue, "Введите время");

        Console.WriteLine(prompt);

        var input = ReadTextInput();
        if (string.IsNullOrWhiteSpace(input) && defaultValue != default)
        {
            timeSpan = defaultValue.Value;

            return true;
        }

        if (TimeSpan.TryParse(input, out timeSpan))
        {
            return true;
        }

        Console.WriteLine("Неподдерживаемый формат времени");

        return false;
    }

    public DateTime ReadDateTimeUntilValid(string prompt = default, DateTime? defaultValue = default)
    {
        while (true)
        {
            if (TryReadDateTime(out var value, prompt, defaultValue))
            {
                return value;
            }
        }
    }

    public bool TryReadDateTime(out DateTime dateTime, string prompt = default, DateTime? defaultValue = default)
    {
        prompt = CreatePrompt(prompt, defaultValue?.ToLocalTime(), "Введите дату");

        Console.WriteLine(prompt);

        var input = ReadTextInput();
        if (string.IsNullOrWhiteSpace(input) && defaultValue != default)
        {
            dateTime = defaultValue.Value;

            return true;
        }

        if (TryParseDateTimeWithNow(input, out dateTime))
        {
            return true;
        }

        Console.WriteLine("Неподдерживаемый формат даты");

        return false;
    }

    public bool TryReadInt(out int value, string prompt = default, int? defaultValue = default)
    {
        prompt = CreatePrompt(prompt, defaultValue, "Введите число");
        Console.WriteLine(prompt);

        var input = ReadTextInput();

        if (int.TryParse(input, out value))
        {
            return true;
        }

        Console.WriteLine("Введите корректное число");

        return false;
    }

    public string ReadStringUntilNonEmpty(string prompt = default, string defaultValue = default)
    {
        string value;
        while (true)
        {
            value = ReadString(prompt, defaultValue);
            if (!string.IsNullOrWhiteSpace(value))
            {
                break;
            }

            if (defaultValue != default)
            {
                return defaultValue;
            }

            Console.WriteLine("Нельзя пустое или пробельное значение, попробуйте еще раз.");
        }

        return value;
    }

    public string ReadString(string prompt = default, string defaultValue = default)
    {
        prompt = CreatePrompt(prompt, defaultValue, "Введите текст");
        Console.WriteLine(prompt);

        return ReadTextInput();
    }

    private bool TryParseDateTimeWithNow(string input, out DateTime dateTime)
    {
        if (string.Equals(input,    "*",   StringComparison.Ordinal)
            || string.Equals(input, "now", StringComparison.OrdinalIgnoreCase))
        {
            dateTime = DateTime.UtcNow;

            return true;
        }

        if (DateTime.TryParse(input,  out dateTime))
        {
            dateTime = dateTime.ToUniversalTime();
            return true;
        }

        return false;
    }

    private static string CreatePrompt(string prompt, object defaultValue, string defaultPrompt)
    {
        prompt ??= defaultPrompt;
        if (defaultValue != default)
        {
            prompt += $"({defaultValue})";
        }

        prompt += ":";

        return prompt;
    }

    private static string ReadTextInput()
    {
        Console.Write("> ");
        var input = Console.ReadLine();

        return input;
    }
}
