using Meeter.Cli.Views;
using Meeter.Services;
using Meeter.Services.Notifications;
using Terminal.Gui;

using var notificationSender = new NotificationSenderFactory()
    .Create();

notificationSender.Start();

Application.Init();

try
{
    var window = new MainWindow();
    var menu = new MenuBarProvider().Get(window);
    Application.Top.Add(menu, window);
    Application.Run();
}
finally
{
    Application.Shutdown();
}
