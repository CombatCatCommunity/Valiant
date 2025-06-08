using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Valiant.Services;

public class DiscordHost(
        DiscordSocketClient discord,
        IConfiguration config,
        ILogger<DiscordHost> logger
    ) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        discord.Log += msg => LogHelper.OnLogAsync(logger, msg);
        discord.GuildAvailable += OnGuildAvailableAsync;

        await discord.LoginAsync(TokenType.Bot, config["discord:token"]);
        await discord.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        discord.GuildAvailable -= OnGuildAvailableAsync;

        await discord.LogoutAsync();
        await discord.StopAsync();
    }

    private async Task OnGuildAvailableAsync(SocketGuild guild)
    {
        await guild.GetStickersAsync();
        await guild.GetEmotesAsync();
    }
}
