using Meeter.Models;

namespace Meeter.Services.Notifications;

public interface INotificationSender
{
    void SendNotification(Meeting meeting);
}
