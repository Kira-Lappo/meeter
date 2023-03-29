using ConsoleTables;
using Meeter.Services;

namespace Meeter.Cli.Services.MeetingPrints;

public class MeetingsPrintService
{
    private string[] _columnNames;
    private readonly MeetingService _meetingService;

    public MeetingsPrintService(MeetingService meetingService)
    {
        _meetingService = meetingService;
        InitColumnNames();
    }

    public void PrintByDate(DateTime date, TextWriter textWriter)
    {
        var meetings = _meetingService.GetAllByStartDate(date);

        var table = new ConsoleTable(_columnNames);
        table.Configure(o =>
        {
            o.OutputTo = textWriter;
        });

        foreach (var m in meetings)
        {
            table.AddRow(
                m.Subject,
                m.StartDateTime,
                m.EndDateTime,
                m.NotifyBeforeTime,
                m.HasBeenNotifiedAbout
            );
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
