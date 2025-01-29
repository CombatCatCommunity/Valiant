using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Valiant.Services;

public class DiscordStartupService(
        DiscordSocketClient discord,
        IConfiguration config,
        ILogger<DiscordStartupService> logger
    ) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        discord.Log += msg => LogHelper.OnLogAsync(logger, msg);
        await discord.LoginAsync(TokenType.Bot, config["discord:token"]);
        await discord.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await discord.LogoutAsync();
        await discord.StopAsync();
    }
}
