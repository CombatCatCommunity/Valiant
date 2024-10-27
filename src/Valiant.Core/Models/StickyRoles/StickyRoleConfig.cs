using LiteDB;

namespace Valiant.Models;

public class StickyRoleConfig
{
    [BsonId]
    public ulong GuildId { get; set; }
    public List<ulong> RoleIds { get; set; }
}
