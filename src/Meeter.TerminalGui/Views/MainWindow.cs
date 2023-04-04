using System.Data;
using Meeter.Models;
using Terminal.Gui;

namespace Meeter.TerminalGui.Views;

public partial class MainWindow
{
    private readonly IDisposable _binding;

    public MainWindow(IViewModel viewModel)
    {
        InitializeComponent();

        _binding = ViewModelBinder.Bind(this, viewModel);

        _meetingTableData.RowDeleted += OnRowDeleted;
        _meetingTableData.RowChanged += OnRowChanged;
        _meetingsTable.CellActivated += OnCellActivated;

        SetMeeterDateLabel(GetSelectedDayDateTime(viewModel));
    }

    [OnViewModelPropertyChanged("Meetings")]
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

    [OnViewModelPropertyChanged("SelectedPeriodDateTime")]
    public void SetMeeterDateLabel(DateTime newValue)
    {
        Title = $"Meetings at {newValue:yyyy-MMM-d, dddd}";
    }



    private void OnCellActivated(TableView.CellActivatedEventArgs obj)
    {
    }

    private void OnRowChanged(object sender, DataRowChangeEventArgs e)
    {

    }

    private void OnRowDeleted(object sender, DataRowChangeEventArgs e)
    {
        var meetingId = e.Row.Field<Guid>("Id");
        if (meetingId == Guid.Empty)
        {
            return;
        }

    }

    protected override void Dispose(bool disposing)
    {
        if (_meetingTableData != default)
        {
            _meetingTableData.RowDeleted -= OnRowDeleted;
            _meetingTableData.RowChanged -= OnRowChanged;
        }

        if (_meetingsTable != default)
        {
            _meetingsTable.CellActivated -= OnCellActivated;
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
        return (DateTime)viewModel["SelectedPeriodDateTime"];
    }
}
