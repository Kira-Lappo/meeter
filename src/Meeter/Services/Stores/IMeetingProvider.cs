using System;
using System.Collections.Generic;
using Meeter.Models;

namespace Meeter.Services.Stores;

public interface IMeetingProvider
{
    IEnumerable<Meeting> FindOverlappingMeetings(DateTime startDateTime, DateTime endDateTime);
}
