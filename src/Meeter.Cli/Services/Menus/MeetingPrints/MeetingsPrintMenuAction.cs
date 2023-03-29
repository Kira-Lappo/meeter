namespace Meeter.Cli.Services.Menus.MeetingPrints;

public class MeetingsPrintMenuAction : IMenuAction
{
    private readonly ConsoleInputReader _inputReader;
    private readonly MeetingsPrintService _printService;

    public MeetingsPrintMenuAction(ConsoleInputReader inputReader, MeetingsPrintService printService)
    {
        _inputReader  = inputReader;
        _printService = printService;
    }

    public void Execute()
    {
        if (_inputReader.TryReadDateTime(out var dateTime, defaultValue: DateTime.UtcNow))
        {
            _printService.PrintByDate(dateTime);
        }
    }
}
