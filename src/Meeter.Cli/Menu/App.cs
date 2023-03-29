namespace Meeter.Cli.Menu;

public class App
{
    private string _menuDisplayText;
    private bool _shouldRun;

    private MainMenu Menu { get; } = new();

    public void Add(string title, string key, Action action)
    {
        Menu.Add(new MenuItem(title, key, action));
    }

    public void Run()
    {
        Init();

        _shouldRun = true;
        while (_shouldRun)
        {
            DisplayMenu();
            var choice = ReadChoice();
            Execute(choice);
        }
    }

    public void RequestStop()
    {
        _shouldRun = false;
    }

    private void Init()
    {
        _menuDisplayText = Menu.GetDisplayText();
    }

    private void Execute(string choice)
    {
        try
        {
            var isHandled = Menu.Handle(choice);
            if (!isHandled)
            {
                Console.WriteLine("Нет такого пункта в меню.");
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Ошибка: {e}");
        }

        Console.WriteLine(new string('-', Console.BufferWidth));
    }

    private string ReadChoice()
    {
        return Console.ReadLine();
    }

    private void DisplayMenu()
    {
        Console.WriteLine(_menuDisplayText);
    }
}
