using Meeter.Services.Stores;

namespace Meeter.Cli.Services.Menus;

public class NewMeetingMenuAction : IMenuAction
{
    private readonly IMeetingStore _meetingStore;
    private readonly MeetingConsoleReader _meetingConsoleReader;

    public NewMeetingMenuAction(
        IMeetingStore meetingStore,
        MeetingConsoleReader meetingConsoleReader)
    {
        _meetingStore         = meetingStore;
        _meetingConsoleReader = meetingConsoleReader;
    }

    public void Execute()
    {
        var meeting = _meetingConsoleReader.Read();
        _meetingStore.Add(meeting);
    }
}
