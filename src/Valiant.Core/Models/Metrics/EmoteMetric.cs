using LiteDB;

namespace Valiant.Models.Metrics;

public class EmoteMetric
{
    public ObjectId Id { get; set; }
    public ulong GuildId { get; set; }
    public ulong ChannelId { get; set; }
    public bool IsVoiceText { get; set; }
    public ulong? ThreadId { get; set; }
    public string ThreadName { get; set; }
    public ulong UserId { get; set; }
    public ulong MessageId { get; set; }
    public ulong EmoteId { get; set; }
    public string EmoteName { get; set; }
    public bool IsSticker { get; set; } = false;
}
