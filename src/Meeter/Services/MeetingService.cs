﻿using System;
using System.Collections.Generic;
using System.Linq;
using Meeter.Models;
using Meeter.Services.Stores;

namespace Meeter.Services;

public class MeetingService : IMeetingService
{
    private readonly IMeetingStore _meetingStore;
    public MeetingService(IMeetingStore meetingStore)
    {
        _meetingStore = meetingStore;
    }

    public IEnumerable<Meeting> GetAllByStartDate(DateTime date)
    {
        return _meetingStore
            .GetAll()
            .Where(m => m.StartDateTime.Date == date.Date);
    }

    public IEnumerable<Meeting> FindOverlappingMeetings(DateTime startDateTime, DateTime endDateTime)
    {
        return _meetingStore
            .GetAll()
            .Where(m => m.OverlapsDateTime(startDateTime) || m.OverlapsDateTime(endDateTime))
            .ToList();
    }
}