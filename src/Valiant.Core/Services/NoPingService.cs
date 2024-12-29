using Discord;
using Discord.WebSocket;
using LiteDB;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Valiant.Services;

public class NoPingService(DiscordSocketClient discord, ILogger<NoPingService> logger) : IHostedService
{
    private readonly DiscordSocketClient _discord = discord;
    private readonly ILogger<NoPingService> _logger = logger;

    //private readonly LiteDatabase _db = new("Filename=./data/moderation.db;Connection=shared");

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _discord.AutoModActionExecuted += OnAutoModAsync;
        //_discord.AutoModRuleDeleted += OnAutoModDeletedAsync;

        _logger.ZLogInformation($"Started");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _discord.AutoModActionExecuted -= OnAutoModAsync;
        //_discord.AutoModRuleDeleted -= OnAutoModDeletedAsync;

        _logger.ZLogInformation($"Stopped");
        return Task.CompletedTask;
    }

    private Task OnAutoModDeletedAsync(SocketAutoModRule rule)
    {
        throw new NotImplementedException();
    }

    private async Task OnAutoModAsync(SocketGuild guild, AutoModRuleAction action, AutoModActionExecutedData data)
    {
        if (data.Rule.Id != 1299792874996764762)
            return;

        var channel = await data.Channel.GetOrDownloadAsync();

        var user = await data.User.GetOrDownloadAsync();
        var embed = new EmbedBuilder()
            .WithAuthor($"@{user.Username} ({user.DisplayName}) said", user.GetDisplayAvatarUrl())
            .WithDescription(data.Content + "\n\n" +
                            "-# You are not allowed to mention this user, however your " +
                            "message was copied because mentions in embeds do not ping.");

        var msg = await channel.SendMessageAsync(embed: embed.Build());

        var pingedId = MentionUtils.ParseUser(data.MatchedContent);
        var pingedUser = guild.GetUser(pingedId);

        var logTo = guild.GetTextChannel(1236756263019216906);
        await logTo.SendMessageAsync($"Someone tried to ping {pingedUser.DisplayName} {msg.GetJumpUrl()}");
    }
}
