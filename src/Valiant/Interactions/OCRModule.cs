using Discord;
using Discord.Interactions;
using System.Diagnostics;
using System.Drawing;
using Tesseract;

namespace Valiant.Interactions;

public class OCRModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly HttpClient _http;

    public OCRModule(HttpClient http)
    {
        _http = http;
    }

    [RequireUserPermission(GuildPermission.ModerateMembers)]
    [SlashCommand("ocr", "Force run OCR for a specific message")]
    public async Task OCRAsync([Summary(description: "A link to the message with attachments to read")]string messageUrl, int index = 0)
    {
        var timer = Stopwatch.StartNew();
        if (!Uri.TryCreate(messageUrl, default, out _))
        {
            await RespondAsync($"The provided string `{messageUrl}` is not a valid url");
            return;
        }

        var noDomain = messageUrl.Replace("https://discord.com/channels/", "")
                                 .Replace("https://canary.discord.com/channels/", "");
        var ids = noDomain.Split('/');
        if (ids.Length > 3)
        {
            await RespondAsync($"The provided url `{messageUrl}` does not point to a Discord message");
            return;
        }

        if (!ulong.TryParse(ids[0], out ulong guildId) && Context.Guild.Id != guildId)
        {
            await RespondAsync($"The provided url `{messageUrl}` needs to point to a message in this server");
            return;
        }

        if (!ulong.TryParse(ids[1], out ulong channelId) && !Context.Guild.TextChannels.Any(x => x.Id == channelId))
        {
            await RespondAsync($"The provided url `{messageUrl}` does not point to a text channel that I can see");
            return;
        }

        if (!ulong.TryParse(ids[2], out ulong msgId))
        {
            await RespondAsync($"The provided url `{messageUrl}` does not point to an existing message");
            return;
        }

        var targetChannel = Context.Guild.GetTextChannel(channelId);
        var targetMsg = await targetChannel.GetMessageAsync(msgId);
        var targetImages = targetMsg.Attachments.Where(x => x.ContentType.StartsWith("image"));

        if (targetImages.Count() == 0)
        {
            await RespondAsync("The provided message does not have an image attachment (I can't do urls yet)");
            return;
        }

        var target = targetImages.ElementAtOrDefault(index);
        if (target == null)
        {
            await RespondAsync($"There is no attachment at the specified index {index} on this message");
            return;
        }

        using var tesseract = new TesseractEngine(@"./tessdata", "eng");
        var bits = await _http.GetByteArrayAsync(target.Url);
        var image = Pix.LoadFromMemory(bits);

        using var page = tesseract.Process(image);
        var resultText = page.GetText();

        timer.Stop();
        var embed = new EmbedBuilder()
            .WithTitle($"OCR Result in {timer.ElapsedMilliseconds}ms with {page.GetMeanConfidence() * 100}% confidence")
            .WithDescription(string.IsNullOrWhiteSpace(resultText) ? "*No Text Found*" : Format.Code(resultText))
            .WithImageUrl(target.Url);

        await RespondAsync(targetMsg.GetJumpUrl(), embeds: [ embed.Build() ]);
    }
}
