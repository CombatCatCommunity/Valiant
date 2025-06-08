using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using ZLogger;

namespace Valiant.Services;

public class CommandsHost(
    DiscordSocketClient discord,
    CommandService commands,
    IServiceProvider services,
    ILogger<CommandsHost> logger
    ) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        commands.Log += msg => LogHelper.OnLogAsync(logger, msg);
        discord.MessageReceived += OnMessageReceivedAsync;

        await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        logger.ZLogInformation($"Loaded {commands.Commands.Count()} command(s)");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        discord.MessageReceived -= OnMessageReceivedAsync;
        return Task.CompletedTask;
    }

    private async Task OnMessageReceivedAsync(SocketMessage message)
    {
        if (message is not SocketUserMessage msg)
            return;
        int argPos = 0;
        if (!msg.HasStringPrefix(Constants.DefaultCommandPrefix, ref argPos, StringComparison.InvariantCultureIgnoreCase))
            return;

        try
        {
            var context = new SocketCommandContext(discord, msg);
            var result = await commands.ExecuteAsync(context, argPos, services);

            if (!result.IsSuccess)
                return;
        } catch
        {

        }
    }
}