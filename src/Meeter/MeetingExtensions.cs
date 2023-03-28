using System;
using Meeter.Models;

namespace Meeter;

public static class MeetingExtensions
{
    public static bool OverlapsDateTime(this Meeting meeting, DateTime dateTime)
    {
        ArgumentNullException.ThrowIfNull(meeting);

        return dateTime.Between(meeting.StartDateTime, meeting.EndDateTime);
    }
}
