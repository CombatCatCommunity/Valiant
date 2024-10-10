namespace Valiant.Models;

public class OCRConfig
{
    public ulong GuildId { get; init; }
    public ulong LogChannelId { get; set; }

    public float WarningThreshold { get; set; } = 0.85f;
    public float AlertThreshold { get; set; } = 0.5f;

    public bool IsServiceEnabled { get; set; } = false;
    public bool IsChannelBlacklist { get; set; } = false;
    public bool IsRoleBlacklist { get; set; } = false;

    public List<ulong> AllowedChannels { get; set; } = new();
    public List<ulong> AllowedRoles { get; set; } = new();
}
