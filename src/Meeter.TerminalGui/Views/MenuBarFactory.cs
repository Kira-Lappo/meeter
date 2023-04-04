using Meeter.TerminalGui.ViewModels;
using Terminal.Gui;

namespace Meeter.TerminalGui.Views;

public class MenuBarFactory
{
    public MenuBar Create(AppViewModel vm)
    {
        var reloadMenuItem = new MenuItem("Re_load",               default, vm.ReloadMeetingsCommand.Execute);
        var addMenuItem = new MenuItem("_New",                     default, NotImplemented);
        var generateMenuItem = new MenuItem("_Generate test data", default, vm.GenerateDummyDataCommand.Execute);

        var meetingsMenuBarItem = new MenuBarItem("_Meetings",
            new[] { reloadMenuItem, addMenuItem, generateMenuItem, });

        var exitMenuBarItem = new MenuItem("_Exit", default, vm.ExitCommand.Execute);
        var fileBarItem = new MenuBarItem("_App", new[] { exitMenuBarItem, });

        var menu = new MenuBar(new[] { fileBarItem, meetingsMenuBarItem, });

        return menu;
    }

    private static Action NotImplemented => () =>
    {
        MessageBox.Query("Not Implemented", "The feature is not implemented yet", "Ok");
    };
}
