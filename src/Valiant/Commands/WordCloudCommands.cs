using Discord;
using Discord.Commands;
using KnowledgePicker.WordCloud;
using KnowledgePicker.WordCloud.Coloring;
using KnowledgePicker.WordCloud.Drawing;
using KnowledgePicker.WordCloud.Layouts;
using KnowledgePicker.WordCloud.Primitives;
using KnowledgePicker.WordCloud.Sizers;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using System.Globalization;

namespace Valiant.Commands;

public class WordCloudCommands(
    ILogger<WordCloudCommands> logger
    ) : ModuleBase<SocketCommandContext>
{
    [Command("wordcloud")]
    public async Task GenerateWordCloudAsync(IGuildChannel channel)
    {
        var contents = new List<string>();
        switch (channel)
        {
            case ITextChannel text:
                logger.LogInformation($"Downloading messages for text channel {channel.GuildId}/{text.Id}");

                var textMsgs = await text.GetMessagesAsync(int.MaxValue).FlattenAsync();
                logger.LogInformation($"Got {textMsgs.Count()} messages");

                contents.AddRange(textMsgs
                    .Where(x => !string.IsNullOrWhiteSpace(x.CleanContent))
                    .Select(x => x.CleanContent));
                break;

            case IForumChannel forum:
                logger.LogInformation($"Downloading messages for forum channel {channel.GuildId}/{forum.Id}");

                var active = await forum.GetActiveThreadsAsync();
                var archived = await forum.GetPublicArchivedThreadsAsync();

                var threads = active.Concat(archived);
                logger.LogInformation($"Got {threads.Count()} threads, {active.Count} active {archived.Count} archived");

                var threadMsgs = new List<IMessage>();
                foreach (var thread in threads)
                {
                    var msgs = await thread.GetMessagesAsync(int.MaxValue).FlattenAsync();
                    threadMsgs.AddRange(msgs);
                }
                logger.LogInformation($"Got {threadMsgs.Count()} messages");

                contents.AddRange(threadMsgs
                    .Where(x => !string.IsNullOrWhiteSpace(x.CleanContent))
                    .Select(x => x.CleanContent));
                break;

            default:
                await ReplyAsync($"The channel {MentionUtils.MentionChannel(channel.Id)} is not a supported channel type.");
                return;
        }

        var words = new List<string>();
        foreach (var content in contents)
            words.AddRange(content
                .ToLower(CultureInfo.InvariantCulture)
                .Split(' ')
                .Where(x => x.Length > 2));

        var rankedWords = words.GroupBy(x => x).Where(x => x.Count() > 5);
        logger.LogInformation($"Got {rankedWords.Count()} words");

        await File.WriteAllTextAsync($"_{channel.Id} wordstats.csv", 
            string.Join('\n', rankedWords.Select(x => $"{x.Key},{x.Count()}")));

        var entries = rankedWords.Select(x => new WordCloudEntry(x.Key, x.Count()));
        var input = new WordCloudInput(entries)
        {
            Width = 1920,
            Height = 1080,
            MinFontSize = 12,
            MaxFontSize = 64
        };

        var sizer = new LogSizer(input);
        using var engine = new SkGraphicEngine(sizer, input);
        var layout = new SpiralLayout(input);
        var colorizer = new RandomColorizer();
        var generator = new WordCloudGenerator<SKBitmap>(input, engine, layout, colorizer);

        using var final = new SKBitmap(input.Width, input.Height);
        using var canvas = new SKCanvas(final);
        canvas.Clear(SKColors.White);
        using var bitmap = generator.Draw();
        canvas.DrawBitmap(bitmap, 0, 0);

        using var data = final.Encode(SKEncodedImageFormat.Png, 100);
        var output = new FileInfo(Path.Join(Environment.CurrentDirectory, $"_{channel.Id} wordcloud.png"));
        using var writer = output.Create();
        data.SaveTo(writer);

        await Context.Channel.SendFileAsync(writer, $"{channel.Id} wordcloud.png");
    }
}
