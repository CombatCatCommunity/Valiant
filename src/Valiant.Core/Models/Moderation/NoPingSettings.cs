using LiteDB;

namespace Valiant.Models.Moderation;

public class NoPingSettings
{
    public ObjectId Id { get; set; }
    public ulong GuildId { get; set; }
    public ulong AutomodId { get; set; }
    public List<ulong> Users { get; set; }
    public List<ulong> AllowRoles { get; set; }
    public List<ulong> AllowChannels { get; set; }
}
