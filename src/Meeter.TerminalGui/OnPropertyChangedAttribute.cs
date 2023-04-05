namespace Meeter.TerminalGui;

[AttributeUsage(AttributeTargets.Method)]
public class OnPropertyChangedAttribute : Attribute
{
    public OnPropertyChangedAttribute(string propertyName)
    {
        PropertyName = propertyName;
    }

    public string PropertyName { get; }
}
