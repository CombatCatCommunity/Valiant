using Discord;
using Discord.Commands;
using LiteDB;
using Valiant.Models;

namespace Valiant.Commands;

public class TwitchCommands : ModuleBase<SocketCommandContext>
{
    [Command("forcetwitchstats")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task ForceTwitchStatsAsync()
    {
        using var db = new LiteDatabase("Filename=./data/twitch.db;ReadOnly=true");
        var stats = db.GetCollection<TwitchStats>().Query()
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

        var channel = Context.Client.GetGuild(1209429896317771796).GetTextChannel(1293324440339484743);
        await channel.SendMessageAsync($"## ShawnStats for {DateTime.Today.AddDays(-1):dddd, MMMM dd}\n" +
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
