using System.Data;
using System.Reactive;
using Meeter.Models;
using Meeter.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Terminal.Gui;

namespace Meeter.TerminalGui.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private readonly IMeetingService _meetingService;
    private readonly DummyDataGenerationService _dummyDataGenerationService;

    public MainWindowViewModel(DummyDataGenerationService dummyDataGenerationService,
        IMeetingService meetingService)
    {
        _dummyDataGenerationService = dummyDataGenerationService;
        _meetingService             = meetingService;

        Meetings = CreateMeetingsTable();

        GenerateDummyDataCommand = ReactiveCommand.Create(GenerateDummyData);
        ExitCommand = ReactiveCommand.Create(Exit);
    }

    [Reactive]
    public DataTable Meetings { get; set; }

    [Reactive]
    public DateTime SelectedPeriodDateTime { get; set; } = DateTime.Today;

    public ReactiveCommand<Unit, Unit> GenerateDummyDataCommand { get; }

    public ReactiveCommand<Unit, Unit> ExitCommand { get; }

    private void Exit()
    {
        Application.RequestStop();
    }

    private void GenerateDummyData()
    {
        _dummyDataGenerationService.Generate();
        ReloadMeetings();
    }

    private void ReloadMeetings()
    {
        var meetings = _meetingService.GetAllByStartDate(SelectedPeriodDateTime).ToList();
        var meetingsTable = CreateMeetingsTable();
        var rows = meetingsTable.Rows;
        foreach (var m in meetings)
        {
            rows.Add(
                m.Id,
                m.Subject,
                m.StartDateTime,
                m.EndDateTime,
                m.NotifyBeforeTime,
                m.HasBeenNotifiedAbout
            );
        }

        Meetings = meetingsTable;
    }

    private DataTable CreateMeetingsTable()
    {
        var table = new DataTable();

        var idColumn = new DataColumn()
        {
            ColumnName = "Id",
            Unique     = true,
            ReadOnly   = true,
        };

        var subjectColumn = new DataColumn()
        {
            Caption    = "Тема",
            ColumnName = "Subject",
        };

        var startDate = new DataColumn()
        {
            Caption    = "Начало",
            ColumnName = "StartDateTime",
            DataType   = typeof(DateTime),
        };

        var endDate = new DataColumn()
        {
            Caption    = "Окончание",
            ColumnName = "EndDateTime",
            DataType   = typeof(DateTime),
        };

        var notifyBefore = new DataColumn()
        {
            Caption    = "Оповещение за",
            ColumnName = "NotifyBeforeTime",
            DataType   = typeof(TimeSpan),
        };

        var isNotified = new DataColumn()
        {
            Caption    = "Уведомление выслано",
            ColumnName = "HasBeenNotifiedAbout",
            DataType   = typeof(bool),
        };

        var columns = table.Columns;
        columns.Add(idColumn);
        columns.Add(subjectColumn);
        columns.Add(startDate);
        columns.Add(endDate);
        columns.Add(notifyBefore);
        columns.Add(isNotified);

        return table;
    }
}
