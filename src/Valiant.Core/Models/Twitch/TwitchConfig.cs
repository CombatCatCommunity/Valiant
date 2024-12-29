namespace Valiant.Models;

public class TwitchConfig
{
    public ulong GuildId { get; set; }
    public ulong? LogChannelId { get; set; }
    public int? ViewershipThreshold { get; set; }
}
