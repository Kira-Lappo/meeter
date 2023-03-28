using System;

namespace Meeter.Models;

public class Meeting
{
    public Guid Id { get; set; }

    public string Subject { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public TimeSpan NotifyBeforeTime { get; set; }

    public DateTime ExpectedNotifDateTime => StartDateTime - NotifyBeforeTime;

    public bool HasBeenNotifiedAbout { get; set; }
}
