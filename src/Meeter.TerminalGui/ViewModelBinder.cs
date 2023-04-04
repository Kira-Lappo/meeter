using System.ComponentModel;
using Terminal.Gui;

namespace Meeter.TerminalGui;

public class ViewModelBinder
{
    private static readonly Dictionary<View, Dictionary<string, Action<object>>> ViewBinds = new ();

    public static IDisposable Bind(View view, IViewModel viewModel)
    {
        viewModel.PropertyChanged += OnViewModelOnPropertyChanged;

        if (!ViewBinds.ContainsKey(view))
        {
            ViewBinds[view] = view.GetPropertyChangeHandlers();
        }

        return new BindDisposable(() =>
        {
            viewModel.PropertyChanged -= OnViewModelOnPropertyChanged;
        });

        void OnViewModelOnPropertyChanged(object o, PropertyChangedEventArgs args)
        {
            OnViewModelPropertyChanged(view, o, args);
        }
    }

    private static void OnViewModelPropertyChanged(View view, object sender, PropertyChangedEventArgs e)
    {
        if (sender is not IViewModel viewModel)
        {
            return;
        }

        if (!ViewBinds.TryGetValue(view, out var handlers))
        {
            return;
        }

        var name = e.PropertyName;
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("PropertyChangedEventArgs must contain PropertyName", nameof(e));
        }

        if (handlers.TryGetValue(name, out var handler))
        {
            var newValue = viewModel[name];
            handler.Invoke(newValue);
        }
    }

    private class BindDisposable : IDisposable
    {
        private readonly Action _dispose;

        public BindDisposable(Action dispose)
        {
            _dispose = dispose;
        }

        public void Dispose()
        {
            _dispose?.Invoke();
        }
    }
}
