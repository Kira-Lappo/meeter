using Meeter.Cli.Services.Menus;

namespace Meeter.Cli;

public class App
{
    private readonly MainMenu _menu = new();
    private string _menuDisplayText;
    private bool _shouldRun;

    public void Add(string title, string key, IMenuAction action)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(action);

        Add(title, key, action.Execute);
    }

    public void Add(string title, string key, Action action)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(action);

        _menu.Add(new MenuItem(title, key, action));
    }

    public void Run()
    {
        Init();

        _shouldRun = true;
        while (_shouldRun)
        {
            PrintMenu();
            var choice = ReadChoice();
            Execute(choice);
            PrintSeparator();
        }
    }

    public void RequestStop()
    {
        _shouldRun = false;
    }

    private void Execute(string choice)
    {
        try
        {
            var isHandled = _menu.Handle(choice);
            if (!isHandled)
            {
                Console.WriteLine("Нет такого пункта в меню.");
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Ошибка: {e}");
        }
    }

    private string ReadChoice()
    {
        return Console.ReadLine();
    }

    private void PrintMenu()
    {
        Console.WriteLine(_menuDisplayText);
    }

    private static void PrintSeparator()
    {
        Console.WriteLine(new string('-', Console.BufferWidth));
    }

    private void Init()
    {
        _menuDisplayText = _menu.GetDisplayText();
    }
}
