using Meeter.Services;
using Meeter.Services.Stores;
using Meeter.TerminalGui.ViewModels;
using Meeter.TerminalGui.Views;
using Terminal.Gui;

Application.Init();

try
{
    var store = new MeetingStoreProvider().Get();
    var meetingService = new MeetingService(store);
    var dgs = new DummyDataGenerationService(store);
    var viewModel = new AppViewModel(store, dgs, meetingService);

    var rootView = new Toplevel();
    var mainView = new MainWindow(viewModel);
    var menu = new MenuBarFactory().Create(viewModel);

    rootView.Add(menu, mainView);

    Application.Run(rootView);
}
finally
{
    Application.Shutdown();
}
