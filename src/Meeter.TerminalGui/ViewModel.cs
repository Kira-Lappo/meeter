
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Meeter.TerminalGui;

public abstract class ViewModel : IViewModel
{
    private readonly Dictionary<string, Func<object>> _propertyBag;

    protected ViewModel()
    {
        _propertyBag = GeneratePropertyBag();
    }

    #region INotifyPropertyChanged

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);

        return true;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion INotifyPropertyChanged

    #region IPropertyBag

    private Dictionary<string, Func<object>> GeneratePropertyBag()
    {
        return this.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .ToDictionary<PropertyInfo, string, Func<object>>(p =>
                p.Name,
                p => () => p.GetValue(this));
    }

    public object this[string name]
    {
        get
        {
            if (_propertyBag.TryGetValue(name, out var getValue))
            {
                return getValue();
            }

            return default;
        }
    }

    #endregion IPropertyBag
}
