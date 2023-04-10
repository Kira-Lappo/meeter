using ReactiveUI;
using Terminal.Gui;

namespace Meeter.TerminalGui.Views;

public abstract class DialogFor<T> : Dialog, IViewFor<T> where T : class
{
    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = value as T
                           ?? throw new ArgumentException($"Should be of {typeof(T)} type", nameof(value));
    }

    public T ViewModel { get; set; }
}
