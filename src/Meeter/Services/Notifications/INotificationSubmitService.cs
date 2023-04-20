namespace Meeter.Services.Notifications;

public interface INotificationSubmitService : IDisposable
{
    void Start();

    void SendNotifications();
}
