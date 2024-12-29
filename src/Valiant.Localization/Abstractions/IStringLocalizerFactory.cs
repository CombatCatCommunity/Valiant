using System.Globalization;

namespace Valiant.Localization;

public interface IStringLocalizerFactory
{
    IStringLocalizer Create(Type resource, CultureInfo culture);

    public IStringLocalizer Create<T>(CultureInfo culture) 
        => Create(typeof(T), culture);
}
