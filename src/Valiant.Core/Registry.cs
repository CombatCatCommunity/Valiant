using AuxLabs.Twitch.Rest;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Valiant.Models;

namespace Valiant;

public static class Registry
{
    public static void FillFaqDb()
    {
        using var db = new LiteDatabase("./data/faq.db");
        var col = db.GetCollection<FaqCategory>();

        var game = new FaqCategory { Name = "Game", Entries = new() };
        game.Entries.AddRange(new List<FaqEntry>
        {
            new() { Id = "G1", Title = "I have suggestions for things that can be added to or changed about the game, where do I post them?", 
                ThreadUrl = "https://canary.discord.com/channels/1209429896317771796/1237222357769129994"},
            new() { Id = "G2", Title = "I found a bug/glitch, where do I report it?", 
                ThreadUrl = "https://canary.discord.com/channels/1209429896317771796/1237222913153830982"},
            new() { Id = "G3", Title = "Someone was hacking in my game, where can I report their account?", 
                ThreadUrl = "https://canary.discord.com/channels/1209429896317771796/1237223176425963520"},
            new() { Id = "G4", Title = "The game won't launch on my PC! What do I do?", 
                ThreadUrl = "https://canary.discord.com/channels/1209429896317771796/1237577904540483615"},
            new() { Id = "G5", Title = "When will this game come to consoles/Steam Deck? When will it support controllers?", 
                ThreadUrl = "https://canary.discord.com/channels/1209429896317771796/1237580699641450526"},
        });

        col.Insert(game);
        


        //var playtest = new FaqCategory { Name = "Playtest", Entries = new() };

        //var cc = new FaqCategory { Name = "Combat Cat", Entries = new() };

        //var discord = new FaqCategory { Name = "Discord", Entries = new() };


    }

    public static IHostBuilder AddDiscord(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddTransient<DiscordRestClient>();
            services.AddSingleton(_ => new DiscordSocketClient(new()
            {
                AlwaysDownloadUsers = false,
                GatewayIntents = GatewayIntents.All,
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 0,
                SuppressUnknownDispatchWarnings = true
            }));
            services.AddSingleton(services =>
            {
                var discord = services.GetRequiredService<DiscordSocketClient>();
                return new InteractionService(discord, new()
                {
                    LogLevel = LogSeverity.Verbose
                });
            });
            services.AddSingleton(new CommandService(new()
            {
                DefaultRunMode = Discord.Commands.RunMode.Async,
                LogLevel = LogSeverity.Verbose
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
