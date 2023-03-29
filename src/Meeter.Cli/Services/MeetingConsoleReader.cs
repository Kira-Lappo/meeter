using Meeter.Models;
using Meeter.Services;

namespace Meeter.Cli.Services;

public class MeetingConsoleReader
{
    private readonly ConsoleInputReader _inputReader;
    private readonly IMeetingService _meetingService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly MeetingsPrintService _meetingsPrintService;

    public MeetingConsoleReader(
        ConsoleInputReader inputReader,
        IMeetingService meetingService,
        IDateTimeProvider dateTimeProvider,
        MeetingsPrintService meetingsPrintService)
    {
        _inputReader          = inputReader;
        _meetingService       = meetingService;
        _dateTimeProvider     = dateTimeProvider;
        _meetingsPrintService = meetingsPrintService;
    }

    public Meeting Read(Meeting defaultValue = default)
    {
        var m = defaultValue?.Clone() ?? new Meeting();

        m.Subject          = _inputReader.ReadStringUntilNonEmpty("Введите тему встречи", m.Subject);
        m                  = RereadUntilValidDates(m);
        m.NotifyBeforeTime = _inputReader.ReadTimeSpanUntilValid("Введите время оповещения", m.NotifyBeforeTime);

        return m;
    }

    private Meeting RereadUntilValidDates(Meeting meeting)
    {
        do
        {
            meeting = ReadMeetingDates(meeting);
        } while (!Validate(meeting));

        return meeting;
    }

    private bool Validate(Meeting meeting)
    {
        var now = _dateTimeProvider.UtcNow;
        if (meeting.StartDateTime <= now || meeting.EndDateTime <= now)
        {
            Console.WriteLine("Дата и время начала и окончания должны быть в будущем периоде.");

            return false;
        }

        if (meeting.EndDateTime <= meeting.StartDateTime)
        {
            Console.WriteLine("Дата и время окончания должна быть позже, чем начало.");

            return false;
        }

        var overlapMeetings = _meetingService
            .FindOverlappingMeetings(meeting.StartDateTime, meeting.EndDateTime)
            .Where(m => m.Id != meeting.Id)
            .ToList();

        if (overlapMeetings.Any())
        {
            Console.WriteLine($"Есть встречи ({overlapMeetings.Count}), конфликутющие с вашей.");
            _meetingsPrintService.PrintAsTable(overlapMeetings);

            return false;
        }

        return true;
    }

    private Meeting ReadMeetingDates(Meeting defaultValue = default)
    {
        var m = defaultValue?.Clone() ?? new Meeting();

        m.StartDateTime = _inputReader.ReadDateTimeUntilValid("Введите дату и время начала встречи", m.StartDateTime)
            .ToUniversalTime();

        m.EndDateTime = _inputReader.ReadDateTimeUntilValid("Введите дату и время окончания встречи", m.EndDateTime)
            .ToUniversalTime();

        return m;
    }
}
