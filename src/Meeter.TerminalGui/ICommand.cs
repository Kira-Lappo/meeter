namespace Meeter.TerminalGui;

public interface ICommand
{
    void Execute();
}

class Command : ICommand
{
    private readonly Action _action;

    public Command(Action action)
    {
        _action = action;
    }

    public void Execute()
    {
        _action.Invoke();
    }
}
