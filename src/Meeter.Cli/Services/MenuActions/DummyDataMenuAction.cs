using Meeter.Models;
using Meeter.Services;
using Meeter.Services.Stores;

namespace Meeter.Cli.Services.MenuActions;

public class DummyDataMenuAction : IMenuAction
{
    private readonly DummyDataGenerationService _meetingStore;

    public DummyDataMenuAction(DummyDataGenerationService meetingStore)
    {
        _meetingStore = meetingStore;
    }

    public void Execute()
    {
        _meetingStore.Generate();

    }
}
