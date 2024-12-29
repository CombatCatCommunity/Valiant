using Discord;
using Discord.Interactions;
using Valiant.Preconditions;

namespace Valiant.Interactions;

[RequireConfigured("assignroles")]
[RequireBotPermission(GuildPermission.ModerateMembers)]
public class AssignModule : InteractionModuleBase<SocketInteractionContext>
{

}
