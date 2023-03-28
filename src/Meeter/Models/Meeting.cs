using System;

namespace Meeter.Models;

public class Meeting
{
    public string Subject { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public TimeSpan NotifyBeforeTime { get; set; }
}
