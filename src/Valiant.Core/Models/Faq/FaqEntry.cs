using LiteDB;

namespace Valiant.Models;

public class FaqEntry
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ThreadUrl { get; set; }
}
