using System.Globalization;

namespace Valiant.Localization;

public class DefaultStringLocalizer<TResource>(IStringLocalizerFactory factory) : IStringLocalizer<TResource>
{
    private readonly IStringLocalizer _localizer 
        = factory.Create(typeof(TResource), CultureInfo.CurrentCulture);

    public LocalizedString this[string name] => _localizer[name];
    public LocalizedString this[string name, params object[] args] => _localizer[name, args];
    public IEnumerable<LocalizedString> GetAllStrings() => _localizer.GetAllStrings();
}
