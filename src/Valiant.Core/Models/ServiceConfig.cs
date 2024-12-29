using LiteDB;

namespace Valiant.Models;

public class ServiceConfig
{
    [BsonId]
    public ulong GuildId { get; set; }
    public bool? IsDisabled { get; set; }

    public List<ulong> RoleIds { get; set; }
    public bool? IsRoleBlacklist { get; set; }

    public List<ulong> ChannelIds { get; set; }
    public bool? IsChannelBlacklist { get; set; }
}
