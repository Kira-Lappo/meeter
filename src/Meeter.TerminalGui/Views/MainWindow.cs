using System.Data;
using Meeter.Models;
using Terminal.Gui;

namespace Meeter.TerminalGui.Views;

public partial class MainWindow
{
    private readonly IViewModel _viewModel;
    private const string SelectedPeriodDateTimePropName = "SelectedPeriodDateTime";
    private const string MeetingsPropName = "Meetings";

    private readonly IDisposable _binding;

    public MainWindow(IViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _binding = ViewModelBinder.Bind(this, viewModel);

        _meetingsTable.CellActivated += OnCellActivated;

        SetTitle(GetSelectedDayDateTime(viewModel));
    }

    [OnPropertyChanged(MeetingsPropName)]
    public void SetMeetings(IEnumerable<Meeting> meetings)
    {
        var table = _meetingsTable.Table;

        table.Clear();
        foreach (var meeting in meetings)
        {
            table.Rows.Add(
                meeting.Id,
                meeting.Subject,
                meeting.StartDateTime,
                meeting.EndDateTime,
                meeting.NotifyBeforeTime,
                meeting.HasBeenNotifiedAbout
            );
        }

        UpdateTable();
    }

    [OnPropertyChanged(SelectedPeriodDateTimePropName)]
    public void SetTitle(DateTime newValue)
    {
        Title = $"Meetings at {newValue:yyyy-MMM-d, dddd}";
    }

    private void OnCellActivated(TableView.CellActivatedEventArgs args)
    {
        var dataRow = args.Table.Rows[args.Row];
        var dialog = CreateMeetingEditDialog(dataRow);
        Application.Run(dialog);
    }

    protected override void Dispose(bool disposing)
    {
        if (_meetingTableData != default)
        {
        }

        if (_meetingsTable != default)
        {
            _meetingsTable.CellActivated += OnCellActivated;
        }

        _binding?.Dispose();

        base.Dispose(disposing);
    }

    private void UpdateTable()
    {
        _meetingsTable.Update();
    }

    private static DateTime GetSelectedDayDateTime(IViewModel viewModel)
    {
        return (DateTime)viewModel[SelectedPeriodDateTimePropName];
    }
}
