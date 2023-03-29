using Meeter.Cli.Menu;
using Meeter.Cli.Services;
using Meeter.Cli.Services.MeetingExports;
using Meeter.Cli.Services.MeetingPrints;
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
        var printActionService = new MeetingsPrintMenuActionService(inputReader, printService);

        var meetingExportService = new MeetingJsonExportService();
        var exportActionService = new MeetingExportActionService(meetingExportService, inputReader, meetingService, dateTimeProvider);

        app.Add("Добавить", "1", () => { Console.WriteLine("Not Implemented"); });
        app.Add("Изменить", "2", () => { Console.WriteLine("Not Implemented"); });
        app.Add("Удалить",  "3", () => { Console.WriteLine("Not Implemented"); });

        app.Add("Встречи по дате", "4", printActionService.Execute);

        app.Add("Экспорт", "9", exportActionService.Execute);

        app.Add("Выход", "0", app.RequestStop);
    }
}
