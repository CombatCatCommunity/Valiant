using Discord;
using Discord.Interactions;

namespace Valiant.Converters;

public class StringTimeConverter : TypeConverter<StringTime>
{
    public override ApplicationCommandOptionType GetDiscordType() => ApplicationCommandOptionType.String;

    public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
        => StringTimeTypeReader.ReadAsync(option.Value?.ToString());
}

public class StringTimeComponentConverter : ComponentTypeConverter<StringTime> 
{
    public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IComponentInteractionData option, IServiceProvider services)
        => StringTimeTypeReader.ReadAsync(option.Value?.ToString());
}

public class StringTimeTypeReader : TypeReader<StringTime>
{
    public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, string option, IServiceProvider services)
        => ReadAsync(option);

    public static Task<TypeConverterResult> ReadAsync(string option)
    {
        if (string.IsNullOrWhiteSpace(option))
            return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.BadArgs, "A time duration value must be provided"));
        
        try
        {
            var result = StringTime.Parse(option);
            return Task.FromResult(TypeConverterResult.FromSuccess(result));
        }
        catch (Exception ex)
        {
            return Task.FromResult(TypeConverterResult.FromError(ex));
        }
    }
}