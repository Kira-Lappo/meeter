namespace Meeter.TerminalGui;

[AttributeUsage(AttributeTargets.Method)]
public class OnViewModelPropertyChangedAttribute : Attribute
{
    public OnViewModelPropertyChangedAttribute(string propertyName)
    {
        PropertyName = propertyName;
    }

    public string PropertyName { get; }
}
