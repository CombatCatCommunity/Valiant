using Discord;

namespace Valiant;

public static class Constants
{
    public const GatewayIntents ConfiguredIntents = GatewayIntents.Guilds | GatewayIntents.GuildBans |
        GatewayIntents.GuildMessages | GatewayIntents.GuildEmojis | GatewayIntents.GuildMessageReactions |
        GatewayIntents.GuildVoiceStates | GatewayIntents.GuildWebhooks | GatewayIntents.MessageContent |
        GatewayIntents.GuildMembers | GatewayIntents.AutoModerationActionExecution | GatewayIntents.AutoModerationConfiguration;

    public const string WildAssaultTwitchId = "538724705";
    public const string WildAssaultIgdbId = "288461";

    public const string DefaultCommandPrefix = "v!";

    // Supports up to 5 digit ids; eg #12345, #2, not #123456
    // Place trigger word at the beginning; eg issue #1, faq 7
    // First regex group should always be the id number
    public const string InMessageCommandRegex = @"\D{,2}(\d{1,5})";

    public const string ConnectionStringFormat = "Filename=./data/{0}.db;Connection=shared";

    public static string GetConnectionString(string model, bool isReadOnly = false)
    {
        string value = string.Format(ConnectionStringFormat, model);
        if (isReadOnly) value += ";ReadOnly=true";
        return value;
    }
}
