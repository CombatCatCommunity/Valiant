using AuxLabs.Twitch.Rest;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Valiant;

public static class Registry
{
    public static IHostBuilder AddDiscord(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddTransient<DiscordRestClient>();
            services.AddSingleton(_ => new DiscordSocketClient(new()
            {
                AlwaysDownloadUsers = true,
                GatewayIntents = Constants.ConfiguredIntents,
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 0,
                SuppressUnknownDispatchWarnings = true
            }));
            services.AddSingleton(services =>
            {
                var discord = services.GetRequiredService<DiscordSocketClient>();
                return new InteractionService(discord, new()
                {
                    LogLevel = LogSeverity.Verbose,
                    LocalizationManager = new JsonLocalizationManager("locales/_interactions", "interactions")
                });
            });
            services.AddSingleton(new CommandService(new()
            {
                DefaultRunMode = Discord.Commands.RunMode.Async,
                LogLevel = LogSeverity.Verbose,
                CaseSensitiveCommands = false
            }));
        });

        return builder;
    }

    public static IHostBuilder AddTwitch(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(services =>
            {
                var config = services.GetRequiredService<IConfiguration>();
                return new TwitchRestClient(new TwitchRestConfig
                {
                    ClientId = config["twitch:client_id"],
                    ClientSecret = config["twitch:client_secret"]
                });
            });
        });

        return builder;
    }
}
