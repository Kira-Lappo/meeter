using Meeter.Models;
using Meeter.Services.Stores;
using Terminal.Gui;

namespace Meeter.Cli.Views;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void UpdateMeetings(IEnumerable<Meeting> meetings)
    {
        var table = _meetingsTable.Table;
        table.Clear();
        foreach (var meeting in meetings)
        {
            table.Rows.Add(
                meeting.Subject,
                meeting.StartDateTime,
                meeting.EndDateTime,
                meeting.NotifyBeforeTime,
                meeting.HasBeenNotifiedAbout
                );
        }

        _meetingsTable.Update();
    }
}
