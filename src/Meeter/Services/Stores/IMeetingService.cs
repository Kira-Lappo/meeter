using System;
using System.Collections.Generic;
using Meeter.Models;

namespace Meeter.Services.Stores;

public interface IMeetingService
{
    IEnumerable<Meeting> GetByStartDate(DateTime date);

    IEnumerable<Meeting> FindOverlappingMeetings(DateTime startDateTime, DateTime endDateTime);
}
