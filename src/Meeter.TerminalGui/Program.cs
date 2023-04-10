using System.Reactive.Concurrency;
using Meeter.Services;
using Meeter.Services.Stores;
using Meeter.TerminalGui.ViewModels;
using Meeter.TerminalGui.Views;
using ReactiveUI;
using Splat.ModeDetection;
using Terminal.Gui;

// https://www.reactiveui.net/docs/guidelines/framework/performance-optimization
Splat.ModeDetector.OverrideModeDetector(Mode.Run);

RxApp.TaskpoolScheduler = TaskPoolScheduler.Default;

Application.Init();

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
