using AuxLabs.Twitch;
using AuxLabs.Twitch.Rest;
using Discord.WebSocket;
using LiteDB;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Valiant.Models;

namespace Valiant.Services;

public class TwitchWatcher : BackgroundService
{
    public const int ViewerThreshold = 50;
    public int TotalChecks => _totalChecks;

    private readonly ILogger<TwitchWatcher> _logger;
    private readonly DiscordSocketClient _discord;
    private readonly TwitchRestClient _twitch;
    private readonly LiteDatabase _db;

    private IEnumerable<string> _alreadyNotified;
    private int _totalChecks = 0;
    private int _statsSentForDay = -1;

    private SocketTextChannel _channel;

    public TwitchWatcher(ILogger<TwitchWatcher> logger, DiscordSocketClient discord, TwitchRestClient twitch)
    {
        _logger = logger;
        _discord = discord;
        _twitch = twitch;
        _db = new("./data/twitch.db");

        _discord.Ready += OnReadyAsync;
    }

    private Task OnReadyAsync()
    {
        _channel = _discord.GetGuild(1209429896317771796).GetTextChannel(1293324440339484743);
        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_twitch.Identity == null)
        {
            await _twitch.ValidateAsync();
            _logger.LogInformation("Twitch authenticated");
        }
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        _logger.LogInformation("Started");
        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                if (_channel == null)
                {
                    _logger.LogInformation("Log channel was null, skipping.");
                    continue;
                }

                if (DateTime.UtcNow.Hour == 16 && _statsSentForDay != DateTime.UtcNow.DayOfYear)
                    await SendStatsAsync();

                _totalChecks++;
                _logger.LogInformation("Starting check #" + _totalChecks);

                var results = await _twitch.GetBroadcastsAsync(gameIds: [Constants.WildAssaultTwitchId], count: int.MaxValue).FlattenAsync();

                var aboveThreshold = results.Where(x => x.ViewerCount >= ViewerThreshold).OrderByDescending(x => x.ViewerCount);

                var mostPopular = aboveThreshold?.FirstOrDefault();
                var col = _db.GetCollection<TwitchStats>();
                var stat = new TwitchStats
                {
                    StreamerCount = results.Count(),
                    TotalViewers = results.Sum(x => x.ViewerCount),
                    MostPopularChannel = new(mostPopular?.User.Name ?? "", mostPopular?.ViewerCount ?? 0)
                };
                col.Insert(stat);

                var newlyDetected = aboveThreshold.Select(x => x.Id).Except(_alreadyNotified ?? []);
                if (!newlyDetected.Any())
                {
                    _logger.LogInformation("No new streams detected, skipping.");
                    continue;
                }

                var notifyNow = aboveThreshold.Where(x => newlyDetected.Any(y => y == x.Id));
                _alreadyNotified = aboveThreshold.Select(x => x.Id);

                var embed = new Discord.EmbedBuilder()
                    .WithTitle("Top Streams for Wild Assault")
                    .WithUrl("https://www.twitch.tv/directory/category/wild-assault")
                    .WithDescription(string.Join('\n', notifyNow.Select(x => $"{x.Title}\n[{x.User.DisplayName}](https://twitch.tv/{x.User.Name}) with {x.ViewerCount} views\n")))
                    .WithCurrentTimestamp();

                await _channel.SendMessageAsync($"Found {results.Count()} channel(s), " +
                    $"{aboveThreshold.Count()} with more than {ViewerThreshold} viewers, " +
                    $"and {notifyNow.Count()} not seen last check.", embed: embed.Build());

            } catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }

    public async Task SendStatsAsync()
    {
        _logger.LogInformation("Outputting ShawnStats");
        _statsSentForDay = DateTime.UtcNow.DayOfYear;

        var stats = _db.GetCollection<TwitchStats>().Query()
            .Where(x => x.Timestamp > DateTime.UtcNow.AddDays(-1)).ToList();

        var orderedStreams = stats.OrderByDescending(x => x.StreamerCount);
        var maxStreams = orderedStreams.FirstOrDefault();
        var minStreams = orderedStreams.LastOrDefault();
        double avgStreams = stats.DefaultIfEmpty().Average(x => x?.StreamerCount ?? 0);

        var orderedViews = stats.OrderByDescending(x => x.TotalViewers);
        var maxViewers = orderedViews.FirstOrDefault();
        var minViewers = orderedViews.LastOrDefault();
        double avgViewers = stats.DefaultIfEmpty().Average(x => x?.TotalViewers ?? 0);

        var popularity = stats.OrderByDescending(x => x.MostPopularChannel.ViewerCount).FirstOrDefault();

        await _channel.SendMessageAsync($"## ShawnStats for {DateTime.Today.AddDays(-1):dddd, MMMM dd}\n" +
            $"**Streams**\n" +
            $"**Max**: {maxStreams?.StreamerCount ?? 0} at <t:{new DateTimeOffset(maxStreams?.Timestamp ?? DateTime.MinValue).ToUnixTimeSeconds()}:t>\n" +
            $"**Min**: {minStreams?.StreamerCount ?? 0} at <t:{new DateTimeOffset(minStreams?.Timestamp ?? DateTime.MinValue).ToUnixTimeSeconds()}:t>\n" +
            $"**Avg**: {Math.Round(avgStreams, 1)}\n" +
            $"**Viewers**\n" +
            $"**Max**: {maxViewers?.TotalViewers ?? 0} at <t:{new DateTimeOffset(maxViewers?.Timestamp ?? DateTime.MinValue).ToUnixTimeSeconds()}:t>\n" +
            $"**Min**: {minViewers?.TotalViewers ?? 0} at <t:{new DateTimeOffset(minViewers?.Timestamp ?? DateTime.MinValue).ToUnixTimeSeconds()}:t>\n" +
            $"**Avg**: {Math.Round(avgViewers, 1)}\n" +
            $"**Most Popular Stream:** <https://twitch.tv/{popularity?.MostPopularChannel.Name}> " +
                $"at <t:{new DateTimeOffset(popularity?.Timestamp ?? DateTime.MinValue).ToUnixTimeSeconds()}:t>");
    }
}
