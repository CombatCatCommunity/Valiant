using LiteDB;

namespace Valiant.Models;

public class InfoTag
{
    public ObjectId Id { get; set; }
    public ulong GuildId { get; set; }
    public string Name { get; set; }
    public List<InfoEntry> Entries { get; set; } = [];
}
