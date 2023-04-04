namespace Meeter.TerminalGui;

public interface IPropertyBag
{
    object this[string name] { get; }
}
