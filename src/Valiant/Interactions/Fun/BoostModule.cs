using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using LiteDB;
using Valiant.Models;

namespace Valiant.Interactions.Fun;

[Group("boost", "Server booster exclusive commands")]
[RequireRole("Server Booster")]
[RequireContext(ContextType.Guild)]
public class BoostModule : InteractionModuleBase<SocketInteractionContext>, IDisposable
{
    private readonly LiteDatabase _db = new(Constants.GetConnectionString("guilds"));

    public void Dispose() => _db.Dispose();

    [SlashCommand("color", "Choose a role color")]
    public async Task ColorAsync([Autocomplete]string roleName)
    {
        var user = Context.User as SocketGuildUser;
        var role = Context.Guild.Roles.SingleOrDefault(x => x.Name == roleName);
        if (role == null)
        {
            await RespondAsync($"`{roleName}` is not a valid color role", ephemeral: true);
            return;
        }

        if (user.Roles.Contains(role))
        {
            await RespondAsync($"You already have {role.Mention}", ephemeral: true);
            return;
        }

        var settings = _db.GetCollection<BoostSettings>()
            .Query().Where(x => x.GuildId == Context.Guild.Id)
            .SingleOrDefault();
        if (!settings.RoleIds.Contains(role.Id))
        {
            await RespondAsync($"`{roleName}` is not a valid color role", ephemeral: true);
            return;
        }

        var userRoles = user.Roles.Select(x => x.Id).ToList();

        var removeRoleIds = user.Roles.Select(x => x.Id).Intersect(settings.RoleIds);
        if (removeRoleIds.Any())
            userRoles.RemoveAll(x => removeRoleIds.Contains(x));

        userRoles.Add(role.Id);

        await user.ModifyAsync(x =>
        {
            x.RoleIds = userRoles;
        });

        await RespondAsync($"You are now {role.Mention}", ephemeral: true);
    }

    [SlashCommand("colors", "List all available role colors")]
    public async Task ColorsAsync()
    {
        var settings = _db.GetCollection<BoostSettings>()
            .Query().Where(x => x.GuildId == Context.Guild.Id)
            .SingleOrDefault();

        if (settings is null || settings?.RoleIds is null || settings.RoleIds.Count == 0)
        {
            await RespondAsync($"Booster role colors are not configured for this server", ephemeral: true);
            return;
        }

        var roles = Context.Guild.GetRolesById(settings.RoleIds.ToArray());

        var embed = new EmbedBuilder()
            .WithTitle($"Configured Color Roles ({roles.Count()})")
            .WithDescription(string.Join(" ", roles.Select(x => x?.Mention ?? "*not found*")));

        await RespondAsync(embed: embed.Build(), ephemeral: true);
    }

    [AutocompleteCommand("role-name", "color")]
    public async Task SuggestBoostColorsAsync()
    {
        var interaction = (SocketAutocompleteInteraction)Context.Interaction;
        string userInput = interaction.Data.Current.Value.ToString();

        var settings = _db.GetCollection<BoostSettings>()
            .Query().Where(x => x.GuildId == Context.Guild.Id)
            .SingleOrDefault();

        if (settings is null || settings?.RoleIds is null || settings.RoleIds.Count == 0)
        {
            await interaction.RespondAsync(default);
            return;
        }

        var roles = Context.Guild.GetRolesById(settings.RoleIds.ToArray());

        var matches = string.IsNullOrWhiteSpace(userInput)
            ? roles.OrderBy(x => x.Name).Take(25).ToList()
            : roles.Where(x => x.Name.Contains(userInput, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(x => x.Name).Take(25).ToList();

        var results = matches.Select(x => new AutocompleteResult(x.Name.Replace("color: ", ""), x.Name));
        await interaction.RespondAsync(results);
    }
}
