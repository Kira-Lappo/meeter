using System.Reactive.Concurrency;
using Meeter.Services;
using Meeter.Services.Notifications;
using Meeter.Services.Stores;
using Meeter.TerminalGui.Notifications;
using Meeter.TerminalGui.ViewModels;
using Meeter.TerminalGui.Views;
using ReactiveUI;
using Splat.ModeDetection;
using Terminal.Gui;

// https://www.reactiveui.net/docs/guidelines/framework/performance-optimization
Splat.ModeDetector.OverrideModeDetector(Mode.Run);

RxApp.TaskpoolScheduler = TaskPoolScheduler.Default;

Application.Init();
object statusBarUpdateToken = default;

try
{
    var store = new MeetingStoreProvider().Get();
    var meetingService = new MeetingService(store);
    var dgs = new DummyDataGenerationService(store);
    var viewModel = new MainWindowViewModel(dgs, meetingService);

    var rootView = new Toplevel();
    var mainView = new MainWindow(viewModel);
    var menu = new AppMenuBar(viewModel);

    var statusBar = new StatusBar();
    statusBar.Visible = true;

    var statusText = new StatusItem(Key.Null, "STATUS BAR!", null);
    statusBar.AddItemAt(0, statusText);

    rootView.Add(menu, mainView, statusBar);

    var sender = new TerminalGuiNotificationSender(statusBar);
    using var notificationSender = new NotificationSenderFactory()
        .Create(sender);

    statusBarUpdateToken = Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(20), (l) =>
    {
        notificationSender.SendNotifications();
        return true;
    });

    Application.Run(rootView);
}
finally
{
    if (statusBarUpdateToken != default)
    {
        Application.MainLoop.RemoveTimeout(statusBarUpdateToken);
    }

    Application.Shutdown();
}
