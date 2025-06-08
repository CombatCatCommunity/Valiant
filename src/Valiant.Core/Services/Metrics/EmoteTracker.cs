using Discord;
using Discord.WebSocket;
using LiteDB;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Valiant.Models.Metrics;
using ZLogger;

namespace Valiant.Services.Metrics;

/// <summary>
///     Background service to collect metrics on the usage of server emotes and stickers.
/// </summary>
public class EmoteTracker(
        DiscordSocketClient discord,
        ILogger<EmoteTracker> logger
    ) : IHostedService
{
    private readonly LiteDatabase _db = new(Constants.GetConnectionString("metrics"));

    public Task StartAsync(CancellationToken cancellationToken)
    {
        discord.MessageReceived += OnMessageReceivedAsync;

        logger.ZLogInformation($"Started");
        return Task.CompletedTask;  
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        discord.MessageReceived -= OnMessageReceivedAsync;

        logger.ZLogInformation($"Stopped");
        return Task.CompletedTask;
    }

    private Task OnMessageReceivedAsync(SocketMessage message)
    {
        // Not sent by a user
        if (message is not SocketUserMessage msg)
            return Task.CompletedTask;

        // Not in a guild
        if (msg.Channel is not SocketGuildChannel channel)
            return Task.CompletedTask;

        // Null if not a thread channel
        var thread = msg.Channel as SocketThreadChannel;

        var metrics = new List<EmoteMetric>();

        // Stickers are present
        if (msg.Stickers?.Count > 0)
        {
            // Exclude stickers from other guilds
            var stickers = msg.Stickers.Intersect(channel.Guild.Stickers);
            if (!stickers.Any())
                return Task.CompletedTask;

            // Convert to metric object
            foreach (var sticker in msg.Stickers)
            {
                metrics.Add(new()
                {
                    GuildId = channel.Guild.Id,
                    ChannelId = channel.Id,
                    IsVoiceText = channel is SocketVoiceChannel,
                    ThreadId = thread?.Id,
                    ThreadName = thread?.Name,
                    UserId = msg.Author.Id,
                    MessageId = msg.Id,
                    EmoteId = sticker.Id,
                    EmoteName = sticker.Name,
                    IsSticker = true
                });
            }
        }

        // Tags are present
        if (msg.Tags?.Count > 0)
        {
            // Exclude non-emoji tags
            var tags = msg.Tags
                .Where(x => x.Type == TagType.Emoji)
                .Distinct()
                .Select(x => x as Tag<Emote>);

            // Exclude emojis not from this server
            tags = tags.Where(x => channel.Guild.Emotes.Any(y => y.Id == x.Key));

            // If nothing is left return
            if (!tags.Any())
                return Task.CompletedTask;

            // Convert to metric object
            foreach (var tag in tags)
            {
                metrics.Add(new()
                {
                    GuildId = channel.Guild.Id,
                    ChannelId = channel.Id,
                    IsVoiceText = channel is SocketVoiceChannel,
                    ThreadId = thread?.Id,
                    ThreadName = thread?.Name,
                    UserId = msg.Author.Id,
                    MessageId = msg.Id,
                    EmoteId = tag.Value.Id,
                    EmoteName = tag.Value.Name
                });
            }
        }

        // If somehow it reaches here with no metrics, return
        if (metrics.Count == 0)
            return Task.CompletedTask;

        logger.ZLogDebug($"Found {metrics.Count} metric(s) at {channel.Guild.Id}/{channel.Id}/{message.Id}");

        // Save to database
        var table = _db.GetCollection<EmoteMetric>();
        table.InsertBulk(metrics);

        return Task.CompletedTask;
    }
}
