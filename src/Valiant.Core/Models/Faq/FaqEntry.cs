using LiteDB;

namespace Valiant.Models;

public class FaqEntry
{
    public ObjectId Id { get; init; }
    public string Category { get; set; }
    public string Title { get; set; }
    public List<FaqMessage> Messages { get; set; }
    public List<string> Tags { get; set; }
}

public record struct FaqMessage(string Content, string Url, string AttachmentFilePath = null);