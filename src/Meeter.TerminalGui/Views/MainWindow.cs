using System.ComponentModel;
using System.Data;
using Meeter.Models;
using Terminal.Gui;

namespace Meeter.TerminalGui.Views;

public partial class MainWindow
{
    private readonly IViewModel _viewModel;

    private readonly Dictionary<string, Action<object>> _viewModelUpdateHandlers;

    public MainWindow(IViewModel viewModel)
    {
        _viewModel = viewModel;

        InitializeComponent();

        _meetingTableData.RowDeleted += OnRowDeleted;
        _meetingTableData.RowChanged += OnRowChanged;
        _meetingsTable.CellActivated += OnCellActivated;

        viewModel.PropertyChanged += OnViewModelPropChanged;

        _viewModelUpdateHandlers = new()
        {
            { "Meetings", (newValue) => SetMeetings((IEnumerable<Meeting>)newValue)},
            { "SelectedPeriodDateTime", (newValue) => SetMeetings((IEnumerable<Meeting>)newValue)},
        };
    }

    private void OnViewModelPropChanged(object sender, PropertyChangedEventArgs e)
    {
        var name = e.PropertyName;
        HandleUpdate(sender as IViewModel, name);
        Update();
    }

    private void HandleUpdate(IViewModel sender, string name)
    {
        var newValue = sender[name];
        if (_viewModelUpdateHandlers.TryGetValue(name, out var handler))
        {
            handler.Invoke(newValue);
        }
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

        _meetingsTable.Update();
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

        _viewModel.PropertyChanged -= OnViewModelPropChanged;

        base.Dispose(disposing);
    }

    private void Update()
    {
        _meetingsTable.Update();
    }
}
