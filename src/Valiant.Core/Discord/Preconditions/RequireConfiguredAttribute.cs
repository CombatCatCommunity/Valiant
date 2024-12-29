using Discord;
using Discord.Interactions;

namespace Valiant.Preconditions;

public class RequireConfiguredAttribute(string Source) : PreconditionAttribute
{
    public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
    {
        throw new NotImplementedException();
    }
}
