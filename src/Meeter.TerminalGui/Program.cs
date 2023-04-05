using System.Reactive.Concurrency;
using Meeter.Services;
using Meeter.Services.Stores;
using Meeter.TerminalGui.ViewModels;
using Meeter.TerminalGui.Views;
using ReactiveUI;
using Terminal.Gui;

Application.Init();

RxApp.TaskpoolScheduler   = TaskPoolScheduler.Default;

try
{

    var store = new MeetingStoreProvider().Get();
    var meetingService = new MeetingService(store);
    var dgs = new DummyDataGenerationService(store);
    var viewModel = new MainWindowViewModel(dgs, meetingService);

    var rootView = new Toplevel();
    var mainView = new MainWindow(viewModel);
    var menu = new AppMenuBar(viewModel);

    rootView.Add(menu, mainView);

    Application.Run(rootView);
}
finally
{
    Application.Shutdown();
}
