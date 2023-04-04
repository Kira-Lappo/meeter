using Meeter.Models;
using Meeter.Services.Stores;

namespace Meeter.Services;

public class DummyDataGenerationService
{
    private readonly IMeetingStore _meetingStore;

    public DummyDataGenerationService(IMeetingStore meetingStore)
    {
        _meetingStore = meetingStore;
    }

    public void Generate()
    {
        var startPoint = _meetingStore
                             .GetAll()
                             .MaxBy(m => m.EndDateTime)
                             ?.EndDateTime
                             .AddHours(3)
                         ?? DateTime.UtcNow;

        for (var i = -2; i <= 3; i++)
        {
            var startTime = startPoint.AddMinutes(30 * i);
            var endTime = startTime.AddMinutes(15);
            var notifyBefore = TimeSpan.FromMinutes(Math.Abs(9 * i));

            _meetingStore.Add(new Meeting
            {
                Subject              = $"Message #{i}",
                StartDateTime        = startTime,
                EndDateTime          = endTime,
                NotifyBeforeTime     = notifyBefore,
                HasBeenNotifiedAbout = i < 0,
            });
        }
    }
}
