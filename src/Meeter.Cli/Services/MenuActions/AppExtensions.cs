namespace Meeter.Cli.Services.MenuActions;

public static class AppExtensions
{
    public static void Add(this App app, string title, string key, IMenuAction action)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(action);

        app.Add(title, key, action.Execute);
    }
}
