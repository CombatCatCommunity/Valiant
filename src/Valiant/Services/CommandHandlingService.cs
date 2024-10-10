using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Valiant.Commands;
using Valiant.Utility;

namespace Valiant.Services;

public class CommandHandlingService : IHostedService
{
    private readonly DiscordSocketClient _discord;
    private readonly CommandService _commands;
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public CommandHandlingService(
        DiscordSocketClient discord,
        CommandService commands,
        IServiceProvider services,
        ILogger<CommandHandlingService> logger)
    {
        _discord = discord;
        _commands = commands;
        _services = services;
        _logger = logger;

        _commands.Log += msg => LogHelper.OnLogAsync(_logger, msg);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _discord.MessageReceived += OnMessageReceivedAsync;

        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        _logger.LogInformation($"Loaded {_commands.Commands.Count()} command(s)");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _discord.MessageReceived -= OnMessageReceivedAsync;
        return Task.CompletedTask;
    }

    private async Task OnMessageReceivedAsync(SocketMessage message)
    {
        if (message is not SocketUserMessage msg)
            return;
        int argPos = 0;
        if (!msg.HasStringPrefix("vally!", ref argPos))
            return;

        try
        {
            var context = new SocketCommandContext(_discord, msg);
            var result = await _commands.ExecuteAsync(context, argPos, _services);

            if (!result.IsSuccess)
                return;
        } catch
        {

        }
    }
}
