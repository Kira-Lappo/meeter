namespace Meeter.Services.Stores;

public class MeetingStoreProvider : IMeetingStoreProvider
{
    private static readonly MeetingStore MeetingStore = new();

    public IMeetingStore Get()
    {
        return MeetingStore;
    }
}