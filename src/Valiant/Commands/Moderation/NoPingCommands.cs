using Discord;
using Discord.Commands;

namespace Valiant.Commands.Moderation;

[Group("noping")]
[RequireBotPermission(GuildPermission.Administrator)]
[RequireUserPermission(GuildPermission.Administrator)]
public class NoPingCommands : ModuleBase<SocketCommandContext>
{
    [Command]
    public async Task ListAsync()
    {
        // Show current no ping service config
        await Task.Delay(0);
    }

    [Command("enable")]
    public async Task EnableAsync()
    {
        // Create the no ping automod rule
        await Task.Delay(0);
    }

    [Command("disable")]
    public async Task DisableAsync()
    {
        // Delete the no ping automod rule
        await Task.Delay(0);
    }

    [Command("add")]
    public async Task AddAsync(IGuildUser user)
    {
        // Add a user to the automod rule
        await Task.Delay(0);
    }

    [Command("del")]
    public async Task DeleteAsync(IGuildUser user)
    {
        // Remove a user from the automod rule
        await Task.Delay(0);
    }
}
