using Meeter.Services;
using Meeter.Services.Notifications;

using var notificationSender = new NotificationSenderFactory()
    .Create();

notificationSender.Start();

Console.WriteLine("Here must be a menu with adding meetings!");
Console.ReadKey();
