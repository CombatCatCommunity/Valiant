using Discord.WebSocket;

namespace Valiant;

public static class GuildExtensions
{
    public static IEnumerable<SocketRole> GetRolesById(this SocketGuild guild, params ulong[] roleIds)
    {
        var roles = new List<SocketRole>();
        foreach (var roleId in roleIds)
            roles.Add(guild.GetRole(roleId));
        return roles;
    }
}
