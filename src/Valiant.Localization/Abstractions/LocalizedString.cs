namespace Valiant.Localization;

/// <summary>
///     The localized version of a string
/// </summary>
/// <param name="Name">
///     The name of the resource value that was loaded.
/// </param>
/// <param name="Value">
///     The localized string
/// </param>
/// <param name="IsNotFound">
///     Whether a value was found with the specified resource and locale. If true, a default or backup value was used.
/// </param>
/// <param name="SearchedFilePath">
///     The file path that was searched for this localization value.
/// </param>
public record struct LocalizedString(string Name, string Value, bool IsNotFound, string? SearchedFilePath)
{
    public LocalizedString(string name, string value)
        : this(name, value, false, null) { }
    public LocalizedString(string name, string value, bool isNotFound)
        : this(name, value, isNotFound, null) { }

    public override readonly string ToString() => Value;
    public static implicit operator string?(LocalizedString localized) => localized.Value;
}
