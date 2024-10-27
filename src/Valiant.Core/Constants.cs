using Discord;

namespace Valiant;

public static class Constants
{
    public const GatewayIntents ConfiguredIntents = GatewayIntents.Guilds | GatewayIntents.GuildBans | 
        GatewayIntents.GuildMessages | GatewayIntents.GuildEmojis | GatewayIntents.GuildMessageReactions | 
        GatewayIntents.GuildVoiceStates | GatewayIntents.GuildWebhooks | GatewayIntents.MessageContent |
        GatewayIntents.GuildMembers;

    public const string WildAssaultTwitchId = "538724705";
    public const string WildAssaultIgdbId = "288461";

    public const string SapphirePrefix = "saph!";
    public const string ValiantPrefix = "vally!";

    public const string TessdataDirectory = @"./tessdata";

    public const int DefaultMinReminderMinutes = 1;
    public const int DefaultMaxReminderDays = 1825;
}
