namespace Meeter.Cli.Menu;

public class MenuItem
{
    public MenuItem(string title, string key, Action action)
    {
        Title    = title;
        Action   = action;
        Key = key;
    }

    public string Title { get; }

    public string Key { get; }

    public Action Action { get; }
}
