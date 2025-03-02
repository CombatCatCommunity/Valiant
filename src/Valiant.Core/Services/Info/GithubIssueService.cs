using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Octokit;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using ZLogger;

namespace Valiant.Services.Info;

public class GithubIssueService(
    ILogger<GithubIssueService> logger,
    DiscordSocketClient discord,
    GitHubClient github
    ) : IHostedService
{
    private readonly Regex _regex = new("issue " + Constants.InMessageCommandRegex, 
        RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline);
    private readonly ConcurrentDictionary<ulong, DateTime> _lastSpoken = [];

    private Repository _repository;
    private IReadOnlyList<Issue> _issues;
    private DateTime _lastRefreshedAt;
    private int _highestIssueNumber;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        discord.MessageReceived += OnMessageReceivedAsync;

        _repository = await github.Repository.Get(Constants.GithubRepoId);
        await RefreshIssuesAsync();

        logger.ZLogInformation($"Started");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        discord.MessageReceived -= OnMessageReceivedAsync;

        logger.ZLogInformation($"Stopped");
        return Task.CompletedTask;
    }

    private async Task OnMessageReceivedAsync(SocketMessage message)
    {
        if (message is not SocketUserMessage msg)
            return;
        if (message.Channel is not SocketTextChannel channel)
            return;

        if (_lastSpoken.TryGetValue(channel.Id, out var lastSpoken))
        {
            if (lastSpoken.AddSeconds(30) < DateTime.UtcNow)
                return;
        }

        var match = _regex.Match(msg.Content);
        if (!match.Success)
            return;

        string issueString = match.Groups[1]?.Value;
        if (issueString is null || !long.TryParse(issueString, out var issueNumber))
            return;

        var issue = _issues.SingleOrDefault(x => x.Number == issueNumber);

        // If no matching issue cached, and input is higher than cached
        if (issue == null && issueNumber > _highestIssueNumber)
        {
            // Ignore if refreshed recently
            if (_lastRefreshedAt.AddMinutes(1) > DateTime.UtcNow)
                return;

            // Redownload all issues
            await RefreshIssuesAsync();

            // If issue still not found ignore
            issue = _issues.SingleOrDefault(x => x.Number == issueNumber);
            if (issue == null)
                return;
        }

        var replyTo = new MessageReference(msg.Id);
        var embed = new EmbedBuilder()
            .WithTitle(_repository.FullName)
            .WithDescription($"Issue #{issue.Number}: [{issue.Title}]({issue.HtmlUrl})");
        
        await channel.SendMessageAsync(embed: embed.Build(), messageReference: replyTo);
        _lastSpoken.AddOrUpdate(channel.Id, DateTime.UtcNow, (x, y) => DateTime.UtcNow);
    }

    private async Task RefreshIssuesAsync()
    {
        _issues = await github.Issue.GetAllForRepository(Constants.GithubRepoId);
        _highestIssueNumber = _issues.OrderByDescending(x => x.Number).FirstOrDefault()?.Number ?? 0;

        logger.ZLogInformation($"Refreshed `{_issues.Count}` issues, highest number is now `{_highestIssueNumber}`");
        _lastRefreshedAt = DateTime.UtcNow;
    }
}
