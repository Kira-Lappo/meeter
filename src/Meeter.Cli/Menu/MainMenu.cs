namespace Meeter.Cli.Menu;

public class MainMenu
{
    private readonly List<MenuItem> _menuItems = new();

    public bool Handle(string key)
    {
        var menuItem = _menuItems.FirstOrDefault(i =>
            string.Equals(key, i.Key, StringComparison.Ordinal));

        if (menuItem == default)
        {
            return false;
        }

        menuItem.Action?.Invoke();
        return true;
    }

    public void Add(MenuItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _menuItems.Add(item);
    }

    public string GetDisplayText()
    {
        return _menuItems
            .Select(m => $"{m.Key, 2}| {m.Title}")
            .JoinToString(Environment.NewLine);
    }
}
