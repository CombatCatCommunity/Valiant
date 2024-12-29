using System.Globalization;

namespace Valiant.Models;

public class GuildLocale : ServiceConfig
{
    public CultureInfo DefaultCulture { get; set; } = new CultureInfo("en-US");
    public List<ChannelLocale> Channels { get; set; }
}
