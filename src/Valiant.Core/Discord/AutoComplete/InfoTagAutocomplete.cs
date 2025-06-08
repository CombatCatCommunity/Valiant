using Discord;
using Discord.Interactions;
using LiteDB;
using Valiant.Models;

namespace Valiant.Discord;

public class InfoTagAutocomplete : AutocompleteHandler
{
    private readonly LiteDatabase _db = new(Constants.GetConnectionString("info"));

    public override Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
    {
        string value = autocompleteInteraction.Data.Current.Value.ToString();

        var tags = _db.GetCollection<InfoTag>()
            .Query().Where(x => x.GuildId == context.Guild.Id && x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
            .ToList().Take(25);

        if (tags.Count() == 0)
            return Task.FromResult(AutocompletionResult.FromSuccess());
        else
            return Task.FromResult(AutocompletionResult.FromSuccess(tags.Select(x => new AutocompleteResult(x.Name, x.Id.ToString()))));
    }
}
