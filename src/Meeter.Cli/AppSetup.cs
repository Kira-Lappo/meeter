using Meeter.Cli.Services;
using Meeter.Cli.Services.Menus;
using Meeter.Cli.Services.Menus.MeetingEdits;
using Meeter.Cli.Services.Menus.MeetingExports;
using Meeter.Cli.Services.Menus.MeetingPrints;
using Meeter.Cli.Services.Menus.MeetingRemovals;
using Meeter.Cli.Services.Menus.NewMeetings;
using Meeter.Services;
using Meeter.Services.Stores;

namespace Meeter.Cli;

public static class AppSetup
{
    public static void Setup(this App app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var dateTimeProvider = new DateTimeProvider();
        var inputReader = new ConsoleInputReader();
        var meetingStore = new MeetingStoreProvider().Get();
        var meetingService = new MeetingService(meetingStore);
        var printService = new MeetingsPrintService(meetingService);
        var meetingReader = new MeetingConsoleReader(inputReader, meetingService, dateTimeProvider, printService);

        var printActionService = new MeetingsPrintMenuAction(inputReader, printService);
        var meetingConsoleFinder = new MeetingConsoleFinder(inputReader, meetingStore, printService, dateTimeProvider);

        var meetingExportService = new MeetingJsonExportService();
        var exportActionService = new MeetingExportMenuAction(meetingExportService, inputReader, meetingService, dateTimeProvider);

        var newMeetingActionService = new NewMeetingMenuAction(meetingStore, meetingReader);

        var deleteMeetingActionService = new MeetingDeleteMenuAction(meetingStore, meetingConsoleFinder);

        var editMeetingMenuAction = new EditMeetingMenuAction(meetingStore, meetingConsoleFinder, meetingReader);

        var dummyDataMenuAction = new DummyDataMenuAction(meetingStore);

        app.Add("Добавить", "1", newMeetingActionService);
        app.Add("Изменить", "2", editMeetingMenuAction);
        app.Add("Удалить",  "3", deleteMeetingActionService);

        app.Add("Встречи по дате", "4", printActionService);

        app.Add("Экспорт", "5", exportActionService);

        app.Add("Сгенерировать встречи", "9", dummyDataMenuAction);

        app.Add("Выход", "0", app.RequestStop);
    }
}
