namespace Meeter.Services.Stores;

public interface IMeetingStoreProvider
{
    IMeetingStore Get();
}

public class MeetingStoreProvider : IMeetingStoreProvider
{
    private static readonly MeetingStore MeetingStore = new();

    public IMeetingStore Get()
    {
        return MeetingStore;
    }
}
