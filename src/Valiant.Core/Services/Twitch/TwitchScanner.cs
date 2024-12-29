using AuxLabs.Twitch.Rest;
using Discord.WebSocket;
using LiteDB;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using ZLogger;

namespace Valiant.Services;

public class TwitchScanner : IHostedService
{
    private readonly DiscordSocketClient _discord;
    private readonly TwitchRestClient _twitch;
    private readonly ILogger _logger;

    private ConcurrentDictionary<ulong, SocketTextChannel> _logTo;
    private LiteDatabase _db;

    public TwitchScanner(ILogger<TwitchScanner> logger, DiscordSocketClient discord, TwitchRestClient twitch)
    {
        _logger = logger;
        _discord = discord;
        _twitch = twitch;

        _logTo = new();
        _db = new("./data/twitch.db");

        _discord.GuildAvailable += OnGuildAvailableAsync;
        _discord.GuildUnavailable += OnGuildUnavailableAsync;
    }

    private Task OnGuildAvailableAsync(SocketGuild guild)
    {
        // Load _logTo with configured channels
        throw new NotImplementedException();
    }

    private Task OnGuildUnavailableAsync(SocketGuild guild)
    {
        if (_logTo.TryRemove(guild.Id, out var channel))
            _logger.ZLogInformation($"{guild.Name} ({guild.Id}) is unavailable, unloading from service.");
        return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
