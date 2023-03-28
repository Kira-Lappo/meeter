using Meeter.Services.Stores;
using Terminal.Gui;

namespace Meeter.Cli.Views;

public class MenuBarProvider
{
    public MenuBar Get(MainWindow window)
    {
        var menu = new MenuBar(new MenuBarItem[]
        {
            new("_Встречи", new MenuItem[]
            {
                new("_Перезагрузить", string.Empty, () => ReloadMeetings(window)),
                new("_Добавить", string.Empty, NotImplemented),
            }),
            new("_Выход", string.Empty, RequestExitFromApp),
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
