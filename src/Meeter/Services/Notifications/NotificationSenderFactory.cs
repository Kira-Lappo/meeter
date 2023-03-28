using Meeter.Services.Stores;

namespace Meeter.Services.Notifications;

public class NotificationSenderFactory : INotificationSenderFactory
{
    public INotificationSender Create()
    {
        var dateTimeProvider = new DateTimeProvider();
        var store = new MeetingStoreProvider().Get();

        return new NotificationSender(store, dateTimeProvider);
    }
}
