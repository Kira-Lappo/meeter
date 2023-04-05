using System.ComponentModel;
using System.Reflection;
using Terminal.Gui;

namespace Meeter.TerminalGui;

public class ViewModelBinder
{
    private static readonly Dictionary<Type, Dictionary<string, Action<View, object>>> ViewBinds = new();

    public static IDisposable Bind(View view, IViewModel viewModel)
    {
        var viewType = view.GetType();
        if (!ViewBinds.ContainsKey(viewType))
        {
            ViewBinds[viewType] = GetPropertyChangedHandlers(viewType);
        }

        viewModel.PropertyChanged += OnViewModelOnPropertyChanged;
        return new DisposeAction(() =>
        {
            viewModel.PropertyChanged -= OnViewModelOnPropertyChanged;
        });

        void OnViewModelOnPropertyChanged(object o, PropertyChangedEventArgs args)
        {
            HandlePropertyChanged(view, o, args);
        }
    }

    private static void HandlePropertyChanged(View view, object sender, PropertyChangedEventArgs e)
    {
        if (sender is not IViewModel viewModel)
        {
            return;
        }

        if (!ViewBinds.TryGetValue(view.GetType(), out var handlers))
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
            handler.Invoke(view, newValue);
        }
    }

    private static Dictionary<string, Action<View, object>> GetPropertyChangedHandlers(Type viewType)
    {
        return viewType
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => m.GetCustomAttributes<OnPropertyChangedAttribute>().Any())
            .ToDictionary<MethodInfo, string, Action<View, object>>(
                m => m.GetCustomAttribute<OnPropertyChangedAttribute>().PropertyName,
                m => (view, newValue) => m.Invoke(view, new[] { newValue }));
    }

    private class DisposeAction : IDisposable
    {
        private readonly Action _dispose;

        public DisposeAction(Action dispose)
        {
            _dispose = dispose;
        }

        public void Dispose()
        {
            _dispose?.Invoke();
        }
    }
}
