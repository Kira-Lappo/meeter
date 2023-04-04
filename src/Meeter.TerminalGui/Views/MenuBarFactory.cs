using Meeter.Services;
using Meeter.Services.Stores;
using Meeter.TerminalGui.ViewModels;
using Terminal.Gui;

namespace Meeter.TerminalGui.Views;

public class MenuBarFactory
{
    public MenuBar Create(AppViewModel vm)
    {
        var reloadMenuItem = new MenuItem("_Перезагрузить",   default, vm.ReloadMeetingsCommand.Execute);
        var addMenuItem = new MenuItem("_Добавить",           default, NotImplemented);
        var generateMenuItem = new MenuItem("С_генерировать", default, vm.GenerateDummyDataCommand.Execute);

        var meetingsMenuBarItem = new MenuBarItem("_Встречи",
            new[] { reloadMenuItem, addMenuItem, new("===", string.Empty, null), generateMenuItem, });

        var exitMenuBarItem = new MenuBarItem("_Выход", default, vm.ExitCommand.Execute);

        var menu = new MenuBar(new[] { meetingsMenuBarItem, exitMenuBarItem, });

        return menu;
    }

    private static Action NotImplemented => () =>
    {
        MessageBox.Query("Not Implemented", "The feature is not implemented yet", "Ok");
    };
}
