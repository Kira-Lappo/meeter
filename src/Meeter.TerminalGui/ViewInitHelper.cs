using System.Reflection;

namespace Meeter.TerminalGui;

public static class ViewInitHelper
{
    public static Dictionary<string, Action<object>> GetPropertyChangeHandlers(
        this Terminal.Gui.View view)
    {
        return view.GetType()
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => m.GetCustomAttributes<OnViewModelPropertyChangedAttribute>().Any())
            .ToDictionary<MethodInfo, string, Action<object>>(
                m => m.GetCustomAttribute<OnViewModelPropertyChangedAttribute>().PropertyName,
                m => newValue => m.Invoke(view, new[] { newValue }));
    }
}
