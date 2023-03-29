using Meeter.Models;
using Meeter.Services;
using Meeter.Services.Stores;

namespace Meeter.Cli.Services;

public class MeetingConsoleFinder
{
    private readonly ConsoleInputReader _consoleInputReader;
    private readonly IMeetingStore _meetingStore;
    private readonly MeetingsPrintService _meetingsPrintService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MeetingConsoleFinder(
        ConsoleInputReader consoleInputReader,
        IMeetingStore meetingStore,
        MeetingsPrintService meetingsPrintService,
        IDateTimeProvider dateTimeProvider)
    {
        _consoleInputReader   = consoleInputReader;
        _meetingStore         = meetingStore;
        _meetingsPrintService = meetingsPrintService;
        _dateTimeProvider     = dateTimeProvider;
    }

    public Meeting Find()
    {
        var defaultDate = _dateTimeProvider.UtcNow;
        if (!_consoleInputReader.TryReadDateTime(out var date, "Введите дату", defaultDate))
        {
            return default;
        }

        var meetings = _meetingStore.GetAll()
            .Where(m => m.StartDateTime.Date == date.Date)
            .ToList();

        if (meetings.Count <= 0)
        {
            Console.WriteLine("Нет встреч на введённую дату");
            return default;
        }

        _meetingsPrintService.PrintAsTable(meetings, showRowNumbers:true);

        if (!_consoleInputReader.TryReadInt(out var index, "Введите номер встречи"))
        {
            return default;
        }

        if (index < 0 || index >= meetings.Count)
        {
            Console.WriteLine($"Неверный номер. Номер должнен быть в пределах от 0 до {meetings.Count - 1}");
            return default;
        }

        return meetings.ElementAt(index);
    }
}
