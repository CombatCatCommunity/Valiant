using Discord;
using Discord.Interactions;
using LiteDB;
using Valiant.Models;

namespace Valiant.Discord;

public class InfoTagTypeConverter : TypeConverter<InfoTag>
{
    private readonly LiteDatabase _db = new(Constants.GetConnectionString("info"));

    public override ApplicationCommandOptionType GetDiscordType()
        => ApplicationCommandOptionType.String;

    public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
    {
        string value = (string)option.Value;

        var tag = _db.GetCollection<InfoTag>()
            .Query().Where(x => x.GuildId == context.Guild.Id && (x.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase) || x.Id.ToString() == value))
            .SingleOrDefault();

        if (tag == null)
            return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.BadArgs, $"An info tag by the name of `{value}` does not exist."));
        else
            return Task.FromResult(TypeConverterResult.FromSuccess(tag));
    }
}
