using LiteDB;

namespace Valiant.Models;

public class StickyRoleUser
{
    public ObjectId Id { get; set; }
    public ulong GuildId { get; set; }
    public ulong UserId { get; set; }
    public List<ulong> RoleIds { get; set; }
}
