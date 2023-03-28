namespace Meeter.Services.Notifications;

public interface INotificationSenderFactory
{
    INotificationSender Create();
}
