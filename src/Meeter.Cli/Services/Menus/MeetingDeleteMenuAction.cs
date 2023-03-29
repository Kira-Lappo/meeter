using Meeter.Services.Stores;

namespace Meeter.Cli.Services.Menus;

public class MeetingDeleteMenuAction : IMenuAction
{
    private readonly IMeetingStore _meetingStore;
    private readonly MeetingConsoleFinder _meetingConsoleFinder;

    public MeetingDeleteMenuAction(
        IMeetingStore meetingStore,
        MeetingConsoleFinder meetingConsoleFinder)
    {
        _meetingStore         = meetingStore;
        _meetingConsoleFinder = meetingConsoleFinder;
    }

    public void Execute()
    {
        var meeting = _meetingConsoleFinder.Find();
        if (meeting == default)
        {
            return;
        }

        _meetingStore.Remove(meeting.Id);

        Console.WriteLine("Встреча удалена");
    }
}
