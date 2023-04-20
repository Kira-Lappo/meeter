namespace Meeter.Services.Notifications;

public interface INotificationSenderFactory
{
    INotificationSubmitService Create(INotificationSender sender);
}
