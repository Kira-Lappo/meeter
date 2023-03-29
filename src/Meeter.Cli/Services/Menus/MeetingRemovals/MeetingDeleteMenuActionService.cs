using Meeter.Cli.Services.Menus.MeetingPrints;
using Meeter.Services;
using Meeter.Services.Stores;

namespace Meeter.Cli.Services.Menus.MeetingRemovals;

public class MeetingDeleteMenuActionService : IMenuActionService
{
    private readonly ConsoleInputReader _consoleInputReader;
    private readonly IMeetingStore _meetingStore;
    private readonly MeetingsPrintService _meetingsPrintService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MeetingDeleteMenuActionService(
        ConsoleInputReader consoleInputReader,
        IMeetingStore meetingStore,
        MeetingsPrintService meetingsPrintService,
        IDateTimeProvider dateTimeProvider)
    {
        _consoleInputReader    = consoleInputReader;
        _meetingStore          = meetingStore;
        _meetingsPrintService  = meetingsPrintService;
        _dateTimeProvider = dateTimeProvider;
    }

    public void Execute()
    {
        var defaultDate = _dateTimeProvider.UtcNow;
        if (!_consoleInputReader.TryReadDateTime(out var date, "Введите дату", defaultDate))
        {
            return;
        }

        var meetings = _meetingStore.GetAll()
            .Where(m => m.StartDateTime.Date == date.Date)
            .ToList();

        if (meetings.Count <= 0)
        {
            Console.WriteLine("Нет встреч на введенную дату");
            return;
        }

        _meetingsPrintService.PrintAsTable(meetings, showRowNumbers:true);

        if (!_consoleInputReader.TryReadInt(out var index, "Введите номер встречи"))
        {
            return;
        }

        if (index < 0 || index >= meetings.Count)
        {
            Console.WriteLine($"Неверный номер. Номер должнен быть в пределах от 0 до {meetings.Count - 1}");
            return;
        }

        var meetingToDelete = meetings.ElementAt(index);
        _meetingStore.Remove(meetingToDelete.Id);

        Console.WriteLine("Встреча удалена");
    }
}
