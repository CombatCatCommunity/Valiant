using Discord;
using Discord.Interactions;
using LiteDB;
using Valiant.Discord;
using Valiant.Models;

namespace Valiant.Interactions.Info;

public class InfoModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("info", "Get some info, duh.")]
    public async Task InfoAsync(
        [Autocomplete(typeof(InfoTagAutocomplete))]
        InfoTag tag,
        [Choice("English", "en"), Choice("Spanish", "es"), Choice("French", "fr"), 
        Choice("Portuguese", "pt"), Choice("Russian", "ru"), Choice("Chinese", "zh")]
        string language = "en",
        IUser mention = null)
    {
        var entry = tag.Entries.SingleOrDefault(x => x.Language.Equals(language, StringComparison.InvariantCultureIgnoreCase));
        if (entry == null)
        {
            await RespondAsync($"Sorry! The info tag `{tag.Name}` doesn't have an entry for this language. " +
                $"Please submit a community suggestion to get a translation added.", ephemeral: true);
            return;
        }

        var embed = new EmbedBuilder()
            .WithTitle($"{tag.Name} ({entry.Language})")
            .WithDescription(entry.Value)
            .WithFooter("Last Edited At")
            .WithTimestamp(entry.EditedAt);

        await RespondAsync(mention != null ? mention.Mention : "", embed: embed.Build());
    }

    // Create
    // Edit
    // Delete
    // Send As Panel

}
