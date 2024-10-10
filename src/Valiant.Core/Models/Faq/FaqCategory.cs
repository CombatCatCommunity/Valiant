using LiteDB;

namespace Valiant.Models;

public class FaqCategory
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string PanelMessageUrl { get; set; }
    public List<FaqEntry> Entries { get; set; }
}
