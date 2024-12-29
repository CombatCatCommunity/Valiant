using LiteDB;

namespace Valiant.Models;

public class Reminder
{
    [BsonId]
    public string Id { get; init; }

    public ulong GuildId { get; set; }
    public ulong ChannelId { get; set; }
    public ulong? ThreadId { get; set; }
    public ulong UserId { get; set; }
    public DateTime RemindAt { get; set; }
    public string Message { get; set; }

    public bool IsExecuted { get; set; } = false;
}
