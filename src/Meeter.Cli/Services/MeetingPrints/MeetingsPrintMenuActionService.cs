namespace Meeter.Cli.Services.MeetingPrints;

public class MeetingsPrintMenuActionService : IMenuActionService
{
    private readonly ConsoleInputReader _inputReader;
    private readonly MeetingsPrintService _printService;

    public MeetingsPrintMenuActionService(ConsoleInputReader inputReader, MeetingsPrintService printService)
    {
        _inputReader  = inputReader;
        _printService = printService;
    }

    public void Execute()
    {
        _inputReader.ReadDateTimeAndExecute(dateTime =>
        {
            _printService.PrintByDate(dateTime, Console.Out);
        });
    }
}
