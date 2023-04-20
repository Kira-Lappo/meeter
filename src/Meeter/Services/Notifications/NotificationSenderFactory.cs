using Meeter.Services.Stores;

namespace Meeter.Services.Notifications;

public class NotificationSenderFactory : INotificationSenderFactory
{
    public INotificationSubmitService Create(INotificationSender sender)
    {
        var dateTimeProvider = new DateTimeProvider();
        var store = new MeetingStoreProvider().Get();

        return new NotificationSubmitService(store, dateTimeProvider, sender);
    }
}
