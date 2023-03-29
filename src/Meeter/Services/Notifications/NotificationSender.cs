using System.Timers;
using Meeter.Models;
using Meeter.Services.Stores;
using Microsoft.Toolkit.Uwp.Notifications;
using Timer = System.Timers.Timer;

namespace Meeter.Services.Notifications;

internal class NotificationSender : INotificationSender
{
    private const int ConversationId = 424242;

    private readonly IMeetingStore _meetingStore;
    private readonly IDateTimeProvider _dateTimeProvider;

    private Timer _timer;

    public NotificationSender(IMeetingStore meetingStore, IDateTimeProvider dateTimeProvider)
    {
        _meetingStore     = meetingStore;
        _dateTimeProvider = dateTimeProvider;
    }

    public void Start()
    {
        // Todo [2023/03/28 kiril] move values to constants or some config
        _timer = new Timer()
        {
            Enabled   = true,
            Interval  = 10 * 1000,
            AutoReset = true,
        };

        _timer.Elapsed += SendNotifications;
        _timer.Start();
    }

    private void SendNotifications(object sender, ElapsedEventArgs e)
    {
        var meetings = GetMeetingsToNotifyAbout();
        foreach (var meeting in meetings)
        {
            SendNotification(meeting);

            meeting.HasBeenNotifiedAbout = true;
            _meetingStore.Update(meeting);
        }
    }

    private IEnumerable<Meeting> GetMeetingsToNotifyAbout()
    {
        var now = _dateTimeProvider.UtcNow;

        return _meetingStore.GetAll()
            .Where(m => ShouldBeSent(m, now));
    }

    private bool ShouldBeSent(Meeting meeting, DateTime dateTimeBorder)
    {
        if (meeting.HasBeenNotifiedAbout)
        {
            return false;
        }

        if (meeting.EndDateTime < dateTimeBorder)
        {
            return false;
        }

        if (meeting.StartDateTime < dateTimeBorder)
        {
            return true;
        }

        var notifyAtDateTime = meeting.StartDateTime - meeting.NotifyBeforeTime;

        return notifyAtDateTime <= dateTimeBorder;
    }

    private void SendNotification(Meeting meeting)
    {
        new ToastContentBuilder()
            .AddArgument("action",         "viewConversation")
            .AddArgument("conversationId", ConversationId)
            .AddText(meeting.Subject)
            .AddText($"Начало в {GetLocalTimeString(meeting.StartDateTime)}")
            .AddText($"Окончание в {GetLocalTimeString(meeting.EndDateTime)}")
            .Show();
    }

    private static string GetLocalTimeString(DateTime dateTime)
    {
        return dateTime.ToLocalTime().ToString("g");
    }

    public void Dispose()
    {
        if (_timer != default)
        {
            _timer.Stop();
            _timer.Elapsed -= SendNotifications;
            _timer?.Dispose();
            _timer = default;
        }
    }
}
