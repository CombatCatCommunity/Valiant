using Discord;
using Discord.Commands;
using LiteDB;
using Valiant.Models;

namespace Valiant.Commands;

[Group("stickyroles")]
[RequireUserPermission(GuildPermission.ManageRoles)]
[RequireBotPermission(GuildPermission.ManageRoles)]
public class StickyRolesCommands : ModuleBase<SocketCommandContext>
{
    private readonly LiteDatabase _db = new("Filename=./data/stickyroles.db;Connection=shared");

    [Command]
    public async Task ListAsync()
    {
        var embed = new EmbedBuilder()
            .WithTitle("Configured Sticky Roles");

        var config = _db.GetCollection<StickyRoleConfig>().FindOne(x => x.GuildId == Context.Guild.Id);
        if (config == null || config.RoleIds.Count == 0)
        {
            embed.Title += " (0)";
            embed.WithDescription("*None*");
        } else
        {
            embed.Title += $" ({config.RoleIds.Count})";
            embed.WithDescription(string.Join(" ", config.RoleIds.Select(MentionUtils.MentionRole)));
        }

        await Context.Channel.SendMessageAsync(embed: embed.Build());
    }

    [Command("add")]
    public async Task AddAsync(params IRole[] roles)
    {
        var configcol = _db.GetCollection<StickyRoleConfig>();
        var config = configcol.FindOne(x => x.GuildId == Context.Guild.Id);
        if (config == null)
        {
            configcol.Insert(new StickyRoleConfig
            {
                GuildId = Context.Guild.Id,
                RoleIds = roles.Select(x => x.Id).ToList()
            });
        } else
        {
            config.RoleIds.AddRange(roles.Select(x => x.Id));
            configcol.Update(config);
        }

        await Context.Channel.SendMessageAsync($"Added {roles.Length} role(s) to the sticky role service.");
    }

    [Command("remove")]
    public async Task RemoveAsync(params IRole[] roles)
    {
        var configcol = _db.GetCollection<StickyRoleConfig>();
        var config = configcol.FindOne(x => x.GuildId == Context.Guild.Id);
        if (config == null)
        {
            await Context.Channel.SendMessageAsync("No roles to be removed.");
        } else
        {
            foreach (var role in roles)
                config.RoleIds.Remove(role.Id);
            configcol.Update(config);
        }

        await Context.Channel.SendMessageAsync($"Removed {roles.Length} role(s) from the sticky role service.");
    }
}
