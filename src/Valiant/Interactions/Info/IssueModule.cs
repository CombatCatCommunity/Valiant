using Discord.Interactions;
using Microsoft.Extensions.Configuration;
using Octokit;

namespace Valiant.Interactions.Info;

public class IssueModule(
    GitHubClient github
    ) : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("issues", "Get the url to a specific issue")]
    public async Task IssuesAsync(long issueId)
    {
        var issue = await github.Issue.Get(Constants.GithubRepoId, issueId);
        await RespondAsync(issue.Url);
    }
}
