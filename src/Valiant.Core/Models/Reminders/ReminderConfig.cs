namespace Valiant.Models;

public class ReminderConfig
{
    public ulong GuildId { get; init; }

    public List<ulong> RoleIds { get; set; }
    public bool IsRoleBlacklist { get; set; } = false;
    public int? MinReminderMinutes { get; set; }
    public int? MaxReminderDays { get; set; }
}
