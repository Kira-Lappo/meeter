using Meeter.Cli.Services.Menus.MeetingPrints;
using Meeter.Services.Stores;

namespace Meeter.Cli.Services.Menus.MeetingRemovals;

public class MeetingDeleteMenuActionService : IMenuActionService
{
    private readonly ConsoleInputReader _consoleInputReader;
    private readonly IMeetingStore _meetingStore;
    private readonly MeetingsPrintService _meetingsPrintService;

    public MeetingDeleteMenuActionService(
        ConsoleInputReader consoleInputReader,
        IMeetingStore meetingStore,
        MeetingsPrintService meetingsPrintService)
    {
        _consoleInputReader   = consoleInputReader;
        _meetingStore         = meetingStore;
        _meetingsPrintService = meetingsPrintService;
    }

    public void Execute()
    {
        if (!_consoleInputReader.TryReadDateTime(out var date, "Введите дату"))
        {
            return;
        }

        var meetings = _meetingStore.GetAll()
            .Where(m => m.StartDateTime.Date == date.Date)
            .ToList();

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
