using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Valiant.Interactions.Fun;

namespace Valiant.Services;

public class InteractionHandlingService(
        DiscordSocketClient discord,
        InteractionService interactions,
        IServiceProvider services,
        ILogger<InteractionHandlingService> logger
    ) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        interactions.Log += msg => LogHelper.OnLogAsync(logger, msg);
        discord.Ready += () => interactions.RegisterCommandsGloballyAsync(true);
        discord.InteractionCreated += OnInteractionAsync;

        await interactions.AddModuleAsync<BoostModule>(services);
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