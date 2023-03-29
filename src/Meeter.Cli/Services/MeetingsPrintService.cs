using ConsoleTables;
using Meeter.Models;
using Meeter.Services;

namespace Meeter.Cli.Services;

public class MeetingsPrintService
{
    private string[] _columnNames;
    private readonly MeetingService _meetingService;

    public MeetingsPrintService(MeetingService meetingService)
    {
        _meetingService = meetingService;
        InitColumnNames();
    }

    public void PrintByDate(DateTime date, bool showRowNumbers = false)
    {
        var meetings = _meetingService.GetAllByStartDate(date);
        PrintAsTable(meetings, showRowNumbers);
    }

    public void PrintAsTable(IEnumerable<Meeting> meetings, bool showRowNumbers = false)
    {
        var columns = _columnNames;
        if (showRowNumbers)
        {
            columns = columns.Prepend("#").ToArray();
        }

        var table = new ConsoleTable(columns);
        table.Configure(o =>
        {
            o.EnableCount = showRowNumbers;
        });

        foreach (var (m, i) in meetings.Select((m, i) => (m, i)))
        {
            var values = new List<object>()
            {
                m.Subject,
                m.StartDateTime,
                m.EndDateTime,
                m.NotifyBeforeTime,
                m.HasBeenNotifiedAbout
            };

            if (showRowNumbers)
            {
                values.Insert(0, i);
            }

            table.AddRow(values.ToArray());
        }

        table.Write(Format.MarkDown);
    }

    private void InitColumnNames()
    {
        _columnNames = new[]
        {
            "Тема",
            "Начало",
            "Окончание",
            "Напоминание за",
            "Оповещение выслано?",
        };
    }
}
