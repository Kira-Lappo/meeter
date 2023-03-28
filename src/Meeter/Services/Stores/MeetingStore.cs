using System;
using System.Collections.Generic;
using System.Linq;
using Meeter.Models;

namespace Meeter.Services.Stores;

internal class MeetingStore : IMeetingStore
{
    private readonly List<Meeting> _meetings = new();

    // FixMe [2023/03/28 kiril] remove dummy data
    public MeetingStore()
    {
        for (int i = 1; i <= 3; i++)
        {
            _meetings.Add(new Meeting
            {
                Id                   = Guid.NewGuid(),
                Subject              = $"Message #{i} ({DateTime.UtcNow.AddSeconds(10 * i)})",
                StartDateTime        = DateTime.UtcNow.AddMinutes(10 * i),
                EndDateTime          = DateTime.UtcNow.AddMinutes(10 * i + 60),
                NotifyBeforeTime     = TimeSpan.FromMinutes(9 * i),
                HasBeenNotifiedAbout = false,
            });
        }
    }

    public IEnumerable<Meeting> GetAll()
    {
        return new List<Meeting>(_meetings);
    }

    public Meeting GetById(Guid id)
    {
        return _meetings.FirstOrDefault(m => m.Id == id);
    }

    public void Add(Meeting meeting)
    {
        ArgumentNullException.ThrowIfNull(meeting);

        _meetings.Add(meeting);
    }

    public void Remove(Guid id)
    {
        var meeting = _meetings.Find(m => m.Id == id);
        if (meeting != default)
        {
            _meetings.Remove(meeting);
        }
    }

    public void Update(Meeting meeting)
    {
        ArgumentNullException.ThrowIfNull(meeting);

        var storedMeeting = _meetings.FirstOrDefault(m => m.Id == meeting.Id);
        if (storedMeeting == default)
        {
            throw new InvalidOperationException($"Can't find meeting with id {meeting.Id}");
        }

        Copy(meeting, storedMeeting);
    }

    private void Copy(Meeting from, Meeting to)
    {
        to.Id                   = from.Id;
        to.Subject              = from.Subject;
        to.StartDateTime        = from.StartDateTime;
        to.EndDateTime          = from.EndDateTime;
        to.HasBeenNotifiedAbout = from.HasBeenNotifiedAbout;
        to.NotifyBeforeTime     = from.NotifyBeforeTime;
    }
}
