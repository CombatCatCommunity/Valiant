using Discord;
using System.Globalization;

namespace Valiant;

public static class Constants
{
    public const GatewayIntents ConfiguredIntents = GatewayIntents.Guilds | GatewayIntents.GuildBans |
        GatewayIntents.GuildMessages | GatewayIntents.GuildEmojis | GatewayIntents.GuildMessageReactions |
        GatewayIntents.GuildVoiceStates | GatewayIntents.GuildWebhooks | GatewayIntents.MessageContent |
        GatewayIntents.GuildMembers | GatewayIntents.AutoModerationActionExecution | GatewayIntents.AutoModerationConfiguration;

    public const string WildAssaultTwitchId = "538724705";
    public const string WildAssaultIgdbId = "288461";

    public const string SapphirePrefix = "saph!";
    public const string ValiantPrefix = "vally!";

    // Supports up to 5 digit ids; eg #12345, #2, not #123456
    // Place trigger word at the beginning; eg issue #1, faq 7
    // First regex group should always be the id number
    public const string InMessageCommandRegex = @"\D{,2}(\d{1,5})"; 

    public const string TessdataDirectory = @"./tessdata";

    public const int DefaultMinReminderMinutes = 1;
    public const int DefaultMaxReminderDays = 1825;

    public const int DefaultStarThreshold = 3;
}
