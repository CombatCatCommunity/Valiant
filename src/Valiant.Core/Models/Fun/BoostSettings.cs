using LiteDB;

namespace Valiant.Models;

public class BoostSettings
{
    public ObjectId Id { get; set; }
    public ulong GuildId { get; set; }
    public List<ulong> RoleIds { get; set; } = [];
}
