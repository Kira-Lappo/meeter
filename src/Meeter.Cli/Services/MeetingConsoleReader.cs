using Meeter.Models;
using Meeter.Services;
using TimeSpan = ABI.System.TimeSpan;

namespace Meeter.Cli.Services;

public class MeetingConsoleReader
{
    private readonly ConsoleInputReader _inputReader;
    private readonly IMeetingService _meetingService;

    public MeetingConsoleReader(
        ConsoleInputReader inputReader,
        IMeetingService meetingService)
    {
        _inputReader    = inputReader;
        _meetingService = meetingService;
    }

    public Meeting Read(Meeting defaultValue = default)
    {
        var m = defaultValue?.Clone() ?? new Meeting();

        m.Subject          = _inputReader.ReadStringUntilNonEmpty("Введите тему встречи", m.Subject);
        m.NotifyBeforeTime = _inputReader.ReadTimeSpanUntilValid("Введите время оповещения", m.NotifyBeforeTime);

        m = RereadUntilValidDates(m);

        return m;
    }

    private Meeting RereadUntilValidDates(Meeting meeting)
    {
        while (true)
        {
            if (meeting.EndDateTime <= meeting.StartDateTime)
            {
                Console.WriteLine("Дата и время окончания должна быть позже, чем начало.");
                meeting = ReadMeetingDates(meeting);

                continue;
            }

            var overlapMeetings = _meetingService
                .FindOverlappingMeetings(meeting.StartDateTime, meeting.EndDateTime)
                .ToList();

            if (!overlapMeetings.Any())
            {
                var message = overlapMeetings
                    .Select(om => $"{om.Subject.TrimWithEllipsis(10),10}|{om.StartDateTime}|{om.EndDateTime}")
                    .JoinToString(Environment.NewLine);

                Console.WriteLine($"Есть встречи ({overlapMeetings.Count}), конфликутющих с вашей.");
                Console.WriteLine(message);

                meeting = ReadMeetingDates(meeting);

                continue;
            }

            break;
        }

        return meeting;
    }

    private Meeting ReadMeetingDates(Meeting defaultValue = default)
    {
        var m = defaultValue?.Clone() ?? new Meeting();

        m.StartDateTime = _inputReader.ReadDateTimeUntilValid("Введите дату и время начала встречи", m.StartDateTime);
        m.EndDateTime   = _inputReader.ReadDateTimeUntilValid("Введите дату и время окончания встречи", m.EndDateTime);

        return m;
    }
}
