using Discord.WebSocket;
using LiteDB;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NTextCat;
using System.Globalization;
using Valiant.Models;
using ZLogger;

namespace Valiant.Services;

public class LocaleDirectorService(
    DiscordSocketClient discord, 
    ILogger<LocaleDirectorService> logger) 
    : IHostedService
{
    private readonly DiscordSocketClient _discord = discord;
    private readonly ILogger<LocaleDirectorService> _logger = logger;

    private readonly LiteDatabase _db = new("Filename=./data/locales.db;Connection=shared");
    private readonly RankedLanguageIdentifier _identifier = new RankedLanguageIdentifierFactory().Load("locales/Core14.profile.xml");

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _discord.MessageReceived += OnMessageReceivedAsync;

        _logger.ZLogInformation($"Started");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _discord.MessageReceived -= OnMessageReceivedAsync;

        _logger.ZLogInformation($"Stopped");
        return Task.CompletedTask;
    }

    private async Task OnMessageReceivedAsync(SocketMessage message)
    {
        if (message.Author.IsBot) return;
        if (message.Channel is not SocketGuildChannel channel) return;
        if (string.IsNullOrWhiteSpace(message.CleanContent)) return;
        // textcat doesn't work that well if the message has less than 5 words
        if (message.CleanContent.Split(' ').Length < 5) return;

        //var config = _db.GetCollection<GuildLocale>().FindOne(x => x.GuildId == channel.Guild.Id);
        //if (config == null)
        //    return;

        //var locale = config.Channels.FirstOrDefault(x => x.Id == channel.Id)?.Culture ?? config.DefaultCulture;

        var languages = _identifier.Identify(message.CleanContent);

        var mostlikely = languages.FirstOrDefault();
        if (mostlikely == null)
            return;

        var culture = new CultureInfo(mostlikely.Item1.Iso639_3);
        _logger.LogInformation($"\nLanguage: {culture.DisplayName}\nMessage: {message.CleanContent}");

        await Task.Delay(0);
    }
}
