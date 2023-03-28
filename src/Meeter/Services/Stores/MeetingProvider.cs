using System;
using System.Collections.Generic;
using System.Linq;
using Meeter.Models;

namespace Meeter.Services.Stores;

class MeetingProvider : IMeetingProvider
{
    private readonly IMeetingStore _meetingStore;

    public MeetingProvider(IMeetingStore meetingStore)
    {
        _meetingStore = meetingStore;
    }

    public IEnumerable<Meeting> FindOverlappingMeetings(DateTime startDateTime, DateTime endDateTime)
    {
        return _meetingStore
            .GetAll()
            .Where(m => m.OverlapsDateTime(startDateTime) || m.OverlapsDateTime(endDateTime))
            .ToList();
    }
}
