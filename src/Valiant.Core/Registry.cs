using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    //LocalizationManager = new JsonLocalizationManager("locales/_interactions", "interactions")
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
}
