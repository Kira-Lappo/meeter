using System;

namespace Meeter.Services.Notifications;

public interface INotificationSender : IDisposable
{
    void Start();
}
