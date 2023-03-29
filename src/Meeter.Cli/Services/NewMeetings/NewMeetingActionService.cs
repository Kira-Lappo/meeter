using Meeter.Services.Stores;

namespace Meeter.Cli.Services.NewMeetings;

public class NewMeetingActionService : IMenuActionService
{
    private readonly IMeetingStore _meetingStore;
    private readonly MeetingConsoleReader _meetingConsoleReader;

    public NewMeetingActionService(
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
