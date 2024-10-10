using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Tesseract;

namespace Valiant.Services;

public class AttachmentProcessor : IHostedService
{
    private readonly ILogger<AttachmentProcessor> _logger;
    private readonly DiscordSocketClient _discord;
    private readonly HttpClient _http;

    private TesseractEngine _tesseract;
    private SocketTextChannel _logTo;

    public AttachmentProcessor(ILogger<AttachmentProcessor> logger, DiscordSocketClient discord, HttpClient http)
    {
        _logger = logger;
        _discord = discord;
        _http = http;

        _discord.Ready += OnReadyAsync;
    }

    private Task OnReadyAsync()
    {
        _logTo = _discord.GetGuild(1209429896317771796).GetTextChannel(1292332722185830400);
        return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (!Directory.Exists(Constants.TessdataDirectory))
        {
            _logger.LogError("Tesseract training data not found, not starting service.");
            return Task.CompletedTask;
        }

        _discord.MessageReceived += OnMessageReceivedAsync;
        _tesseract = new(Constants.TessdataDirectory, "eng");

        _logger.LogInformation("Started");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _discord.MessageReceived -= OnMessageReceivedAsync;
        _tesseract?.Dispose();
        _tesseract = null;

        _logger.LogInformation("Stopped");
        return Task.CompletedTask;
    }

    private async Task OnMessageReceivedAsync(SocketMessage message)
    {
        if (_tesseract == null)
        {
            _logger.LogError("Message handler active but tesseract is null");
            return;
        }

        var targetImages = message.Attachments.Where(x => x.ContentType.StartsWith("image"));
        if (!targetImages.Any())
            return;

        var results = new List<Embed>();
        foreach (var attachment in targetImages)
        {
            var timer = Stopwatch.StartNew();
            var bits = await _http.GetByteArrayAsync(attachment.Url);
            var image = Pix.LoadFromMemory(bits);

            using var page = _tesseract.Process(image);
            
            var resultText = page.GetText();
            if (string.IsNullOrWhiteSpace(resultText))
            {
                _logger.LogInformation($"Skipping an attachment in msg with id {message.Id}, no text found");
                continue;
            }

            var embed = new EmbedBuilder()
                .WithTitle($"OCR Result in {timer.ElapsedMilliseconds}ms with {page.GetMeanConfidence() * 100}% confidence")
                .WithDescription(Format.Code(resultText))
                .WithImageUrl(attachment.Url);
            results.Add(embed.Build());

            timer.Stop();
            _logger.LogInformation($"Processed an image for msg with id {message.Id} in {timer.ElapsedMilliseconds}ms");
        }

        foreach (var embed in results)
            await _logTo.SendMessageAsync(message.GetJumpUrl(), embed: embed);
    }
}
