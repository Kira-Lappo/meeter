using System;

namespace Meeter.Models;

public class Meeting
{
    public Guid Id { get; set; }

    public string Subject { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public TimeSpan NotifyBeforeTime { get; set; }

    public bool HasBeenNotifiedAbout { get; set; }

    public Meeting Clone()
    {
        var clone = new Meeting();
        clone.CopyFrom(this);

        return clone;
    }

    public void CopyFrom(Meeting from)
    {
        ArgumentNullException.ThrowIfNull(from);

        Id                   = from.Id;
        Subject              = from.Subject;
        StartDateTime        = from.StartDateTime;
        EndDateTime          = from.EndDateTime;
        HasBeenNotifiedAbout = from.HasBeenNotifiedAbout;
        NotifyBeforeTime     = from.NotifyBeforeTime;
    }
}
