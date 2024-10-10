namespace Valiant.Models;

public class TwitchStats
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public int StreamerCount { get; set; }
    public int TotalViewers { get; set; }
    public PopularChannel MostPopularChannel { get; set; }
}

public record class PopularChannel(string Name, int ViewerCount);
