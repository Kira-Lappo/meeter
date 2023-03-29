using Meeter.Models;

namespace Meeter.Services;

public interface IMeetingService
{
    IEnumerable<Meeting> GetAllByStartDate(DateTime date);

    IEnumerable<Meeting> FindOverlappingMeetings(DateTime startDateTime, DateTime endDateTime);
}
