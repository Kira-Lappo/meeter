using Meeter.Models;
using Meeter.Services.Stores;

namespace Meeter.Cli.Services.MenuActions;

public class EditMeetingMenuAction : IMenuAction
{
    private readonly IMeetingStore _meetingStore;
    private readonly MeetingConsoleFinder _meetingConsoleFinder;
    private readonly MeetingConsoleReader _meetingConsoleReader;

    public EditMeetingMenuAction(
        IMeetingStore meetingStore,
        MeetingConsoleFinder meetingConsoleFinder,
        MeetingConsoleReader meetingConsoleReader)
    {
        _meetingStore         = meetingStore;
        _meetingConsoleFinder = meetingConsoleFinder;
        _meetingConsoleReader = meetingConsoleReader;
    }

    public void Execute()
    {
        var meeting = _meetingConsoleFinder.Find();
        if (meeting == default)
        {
            return;
        }

        Console.WriteLine("Для обновления поля введите его новое значение, затем Enter.");
        Console.WriteLine("Для пропуска поля нажмите Enter.");

        var updatedMeeting = _meetingConsoleReader.Read(meeting);

        if (NeedAnotherNotification(meeting, updatedMeeting))
        {
            updatedMeeting.HasBeenNotifiedAbout = false;
        }

        _meetingStore.Update(updatedMeeting);

        Console.WriteLine("Встреча обновлена");
    }

    private bool NeedAnotherNotification(Meeting meeting, Meeting updatedMeeting)
    {
        return meeting.StartDateTime != updatedMeeting.StartDateTime
               || meeting.NotifyBeforeTime != updatedMeeting.NotifyBeforeTime;
    }
}
