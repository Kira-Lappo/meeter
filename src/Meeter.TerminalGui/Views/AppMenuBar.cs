using System.Reactive.Disposables;
using System.Reactive.Linq;
using Meeter.TerminalGui.ViewModels;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using Terminal.Gui;

namespace Meeter.TerminalGui.Views;

public class AppMenuBar : Window, IViewFor<MainWindowViewModel>
{
    private readonly CompositeDisposable _disposable = new();
    public AppMenuBar(MainWindowViewModel viewModel)
    {
        ViewModel = viewModel;
        Initialize();
    }

    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = value as MainWindowViewModel
                           ?? throw new ArgumentException("Should be of AppViewModel type", nameof(value));
    }

    public MainWindowViewModel ViewModel { get; set; }

    public void Initialize()
    {
        var exitMenuBarItem = new MenuItem("_Exit", default, () => { Application.RequestStop(); });
        var fileBarItem = new MenuBarItem("_App", new[] { exitMenuBarItem, });

        var addMenuItem = new MenuItem("_New", default, NotImplemented);

        var meetingsMenuBarItem = new MenuBarItem("_Meetings",
            new[] { addMenuItem, });

        var generateMenuItem = new MenuItem("_Generate test data", default, () =>
        {
            ViewModel.GenerateDummyDataCommand.Execute().Wait();
        });

        var testMenuBar = new MenuBarItem("_Test",
            new[] { generateMenuItem, });

        var menu = new MenuBar(new[] { fileBarItem, meetingsMenuBarItem, testMenuBar });

        Add(menu);
    }

    private static Action NotImplemented => () =>
    {
        MessageBox.Query("Not Implemented", "The feature is not implemented yet", "Ok");
    };
}
