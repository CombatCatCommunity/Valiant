namespace Valiant.Localization;

public interface IStringLocalizer<out T> : IStringLocalizer { }
public interface IStringLocalizer
{
    LocalizedString this[string name] { get; }
    LocalizedString this[string name, params object[] args] { get; }

    IEnumerable<LocalizedString> GetAllStrings();

    public LocalizedString GetString(string name) => this[name];
    public LocalizedString GetString(string name, params object[] arguments) => this[name, arguments];
}
