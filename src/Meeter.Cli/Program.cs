using Meeter.Cli;
using Meeter.Services.Notifications;

var sender = new UWPNotificationSender();
using var notificationSender = new NotificationSenderFactory()
    .Create(sender);

notificationSender.Start();

var app = new App();
app.Setup();
app.Run();
