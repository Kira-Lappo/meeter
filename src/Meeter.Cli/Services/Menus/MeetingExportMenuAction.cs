using Meeter.Models;
using Meeter.Services;
using Meeter.Services.Exports;

namespace Meeter.Cli.Services.Menus;

public class MeetingExportMenuAction : IMenuAction
{
    private readonly IMeetingExportService _meetingExportService;
    private readonly ConsoleInputReader _inputReader;
    private readonly IMeetingService _meetingService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MeetingExportMenuAction(
        IMeetingExportService meetingExportService,
        ConsoleInputReader inputReader,
        IMeetingService meetingService,
        IDateTimeProvider dateTimeProvider)
    {
        _meetingExportService = meetingExportService;
        _inputReader          = inputReader;
        _meetingService       = meetingService;
        _dateTimeProvider     = dateTimeProvider;
    }

    public void Execute()
    {
        var now = _dateTimeProvider.UtcNow;
        if (_inputReader.TryReadDateTime(out var dateTime, defaultValue: now))
        {
            Export(dateTime);
        }
    }

    private void Export(DateTime dateTime)
    {
        var meetings = _meetingService.GetAllByStartDate(dateTime)
            .ToList();

        var exportModel = new MeeterExportData
        {
            Items              = meetings,
            ExportDateTime     = _dateTimeProvider.UtcNow,
            MeetingsByDateTime = dateTime,
        };

        var targetFileName = CreateFileName(dateTime);
        using (var stream = new FileStream(targetFileName, FileMode.Create))
        {
            _meetingExportService.Export(stream, exportModel);
        }

        Console.WriteLine($"Данные экспортированы: {targetFileName}");
    }

    private string CreateFileName(DateTime dateTime)
    {
        return Path.Combine(
            AppContext.BaseDirectory,
            $"meeter-data_{dateTime:yyyy-MM-dd--HH-mm-ss}.json"
        );
    }
}
