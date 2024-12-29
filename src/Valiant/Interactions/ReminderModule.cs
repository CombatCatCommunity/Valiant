using Discord;
using Discord.Interactions;

namespace Valiant.Interactions;

[RequireUserPermission(GuildPermission.ManageMessages)]
[Group("reminder", "Have the bot remind you of something after a certain amount of time")]
public class ReminderModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("add", "Add a new reminder")]
    public Task AddAsync(StringTime time, string message)
    {
        return Task.CompletedTask;
    }

    [SlashCommand("list", "List all of your upcoming reminders")]
    public Task ListAsync()
    {
        // Output user reminders in an embed
        // Include a select menu component to delete

        var menu = new SelectMenuBuilder("reminder_delete")
            .WithPlaceholder("Select a reminder to delete");

        return Task.CompletedTask;
    }

    [ComponentInteraction("reminder_delete", true)]
    public Task DeleteAsync(string[] selectedIds)
    {

        return Task.CompletedTask;
    }
}
