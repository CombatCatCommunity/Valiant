using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Valiant.Discord;
using Valiant.Interactions.Fun;
using Valiant.Interactions.Info;
using Valiant.Models;
using ZLogger;

namespace Valiant.Services;

public class InteractionsHost(
        DiscordSocketClient discord,
        InteractionService interactions,
        IServiceProvider services,
        ILogger<InteractionsHost> logger
    ) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        interactions.Log += msg => LogHelper.OnLogAsync(logger, msg);
        discord.Ready += () => interactions.RegisterCommandsGloballyAsync(true);
        discord.InteractionCreated += OnInteractionAsync;

        interactions.AddTypeConverter<InfoTag>(new InfoTagTypeConverter());

        await interactions.AddModuleAsync<BoostModule>(services);
        await interactions.AddModuleAsync<InfoModule>(services);

        logger.ZLogInformation($"Loaded {interactions.SlashCommands.Count} slash command(s)");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        interactions.Dispose();
        return Task.CompletedTask;
    }

    private async Task OnInteractionAsync(SocketInteraction interaction)
    {
        try
        {
            var context = new SocketInteractionContext(discord, interaction);
            var result = await interactions.ExecuteCommandAsync(context, services);

            if (!result.IsSuccess)
                await interaction.RespondAsync(result.ToString(), ephemeral: true);
        } catch
        {
            if (interaction.Type == InteractionType.ApplicationCommand)
            {
                await interaction.GetOriginalResponseAsync()
                    .ContinueWith(msg => msg.Result.DeleteAsync());
            }
        }
    }
}