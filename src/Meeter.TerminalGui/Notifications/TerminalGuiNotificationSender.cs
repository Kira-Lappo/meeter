using Meeter.Models;
using Meeter.Services.Notifications;
using Terminal.Gui;

namespace Meeter.TerminalGui.Notifications;

public class TerminalGuiNotificationSender : INotificationSender
{
    private readonly StatusBar _statusBar;

    public TerminalGuiNotificationSender(StatusBar statusBar)
    {
        _statusBar = statusBar;
    }

    public void SendNotification(Meeting meeting)
    {
        var text = meeting.Subject + " in " + (meeting.StartDateTime - DateTime.UtcNow).Minutes + " minutes";
        _statusBar.Items[0].Title = text;
        _statusBar.Redraw(_statusBar.Bounds);
    }
}
