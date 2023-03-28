using Meeter.Services.Stores;
using Terminal.Gui;

namespace Meeter.Cli.Views;

public class MenuBarProvider
{
    public MenuBar Get(MainWindow window)
    {
        var menu = new MenuBar(new MenuBarItem[]
        {
            new("_Meetings", new MenuItem[]
            {
                new("_Reload Meetings", string.Empty, () => ReloadMeetings(window)),
                new("_Add", string.Empty, NotImplemented),
            }),
            new("_Quit", string.Empty, RequestExitFromApp),
        });

        return menu;
    }

    private void ReloadMeetings(MainWindow window)
    {
        var meetings = new MeetingStoreProvider().Get().GetAll();
        window.UpdateMeetings(meetings);
    }

    private void RequestExitFromApp()
    {
        Application.RequestStop();
    }

    private void NotImplemented()
    {
        MessageBox.Query("Not Implemented", "The feature is not implemented yet", "Ok");
    }
}
