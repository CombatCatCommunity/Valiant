using Discord;
using Discord.Commands;
using LiteDB;
using Valiant.Models;

namespace Valiant.Commands.Fun;

[Group("boost")]
[RequireBotPermission(GuildPermission.ManageRoles)]
[RequireUserPermission(GuildPermission.Administrator)]
public class BoostCommands : ModuleBase<SocketCommandContext>, IDisposable
{
    private readonly LiteDatabase _db = new(Constants.GetConnectionString("guilds"));

    public void Dispose() => _db.Dispose();

    [Command("colors")]
    [Summary("List all configured booster role colors for this server")]
    public async Task ListAsync()
    {
        var settings = _db.GetCollection<BoostSettings>()
            .Query().Where(x => x.GuildId == Context.Guild.Id)
            .SingleOrDefault();

        if (settings is null || settings?.RoleIds is null || settings.RoleIds.Count == 0)
        {
            await ReplyAsync($"Booster role colors are not configured for this server");
            return;
        }

        var roles = Context.Guild.GetRolesById(settings.RoleIds.ToArray());

        var embed = new EmbedBuilder()
            .WithTitle($"Configured Color Roles ({roles.Count()})")
            .WithDescription(string.Join(" ", roles.Select(x => x?.Mention ?? "*not found*")));

        await ReplyAsync(embed: embed.Build());
    }

    [Command("addcolorsbyprefix")]
    [Summary("Add colors to the booster colors selection based on a role name prefix")]
    public async Task AddColorsByPrefixAsync(string prefix)
    {
        var matches = Context.Guild.Roles.Where(x => x.Name.ToLower().StartsWith(prefix.ToLower()));
        if (!matches.Any())
        {
            await ReplyAsync($"No roles matched the prefix `{prefix}`");
            return;
        }

        await AddColorsAsync(matches.ToArray());
    }

    [Command("addcolors")]
    [Summary("Add colors to the booster colors selection")]
    public async Task AddColorsAsync(params IRole[] roles)
    {
        var table = _db.GetCollection<BoostSettings>();
        var settings = table.Query()
            .Where(x => x.GuildId == Context.Guild.Id)
            .SingleOrDefault() 
            ?? new BoostSettings { GuildId = Context.Guild.Id };

        settings.RoleIds.AddRange(roles.Select(x => x.Id));
        settings.RoleIds = settings.RoleIds.Distinct().ToList();

        if (!table.Update(settings))
            table.Insert(settings);

        await ReplyAsync($"Added **{settings.RoleIds.Count}** role(s) to booster colors selection");
    }

    [Command("delcolors")]
    [Summary("Remove colors from the booster colors selection")]
    public async Task DeleteColorsAsync(params IRole[] roles)
    {
        var table = _db.GetCollection<BoostSettings>();
        var settings = table.Query()
            .Where(x => x.GuildId == Context.Guild.Id)
            .SingleOrDefault()
            ?? new BoostSettings { GuildId = Context.Guild.Id };

        settings.RoleIds.RemoveAll(x => roles.SingleOrDefault(y => y.Id == x) != null);
        settings.RoleIds = settings.RoleIds.Distinct().ToList();

        if (!table.Update(settings))
            table.Insert(settings);

        await ReplyAsync($"Removed **{settings.RoleIds.Count}** role(s) from booster colors selection");
    }
}
