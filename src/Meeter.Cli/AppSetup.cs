using Meeter.Cli.Services;
using Meeter.Cli.Services.MenuActions;
using Meeter.Services;
using Meeter.Services.Exports;
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
        var dummyDataGenerator = new DummyDataGenerationService(meetingStore);

        var printActionService = new PrintMeetingsMenuAction(inputReader, printService);
        var meetingConsoleFinder = new MeetingConsoleFinder(inputReader, meetingStore, printService, dateTimeProvider);

        var meetingExportService = new MeetingJsonExportService();
        var exportActionService = new ExportMeetingsMenuAction(meetingExportService, inputReader, meetingService, dateTimeProvider);

        var newMeetingActionService = new CreateMeetingMenuAction(meetingStore, meetingReader);

        var deleteMeetingActionService = new RemoveMeetingMenuAction(meetingStore, meetingConsoleFinder);

        var editMeetingMenuAction = new EditMeetingMenuAction(meetingStore, meetingConsoleFinder, meetingReader);

        var dummyDataMenuAction = new DummyDataMenuAction(dummyDataGenerator);

        app.Add("Добавить", "1", newMeetingActionService);
        app.Add("Изменить", "2", editMeetingMenuAction);
        app.Add("Удалить",  "3", deleteMeetingActionService);

        app.Add("Встречи по дате", "4", printActionService);

        app.Add("Экспорт", "5", exportActionService);

        app.Add("Сгенерировать встречи", "9", dummyDataMenuAction);

        app.Add("Выход", "0", app.RequestStop);
    }
}
