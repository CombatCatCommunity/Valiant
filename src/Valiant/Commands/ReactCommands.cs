using Discord;
using Discord.Commands;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Valiant.Commands;

[Group("react")]
[RequireUserPermission(ChannelPermission.ManageMessages)]
public class ReactCommands : ModuleBase<SocketCommandContext>
{
    private readonly ILogger _logger;

    public ReactCommands(ILogger<ReactCommands> logger)
    {
        _logger = logger;
    }

    [Command("with")]
    public async Task ReactWithAsync(ulong msgId, string emoteText)
    {
        await Context.Message.DeleteAsync();

        var msg = await Context.Channel.GetMessageAsync(msgId);
        if (msg == null)
        {
            _logger.ZLogInformation($"Message `{msgId}` was not found in this channel `{Context.Channel}`.");
            return;
        }

        if (Emote.TryParse(emoteText, out var emote))
        {
            await msg.AddReactionAsync(emote);
            return;
        }
        if (Emoji.TryParse(emoteText, out var emoji))
        {
            await msg.AddReactionAsync(emoji);
            return;
        }

        _logger.ZLogInformation($"Emote `{emoteText}` was not valid.");
    }


    [Command("text")]
    public async Task ReactTextAsync(ulong msgId, string text)
    {
        await Context.Message.DeleteAsync();

        var msg = await Context.Channel.GetMessageAsync(msgId);
        if (msg == null)
        {
            _logger.ZLogInformation($"Message `{msgId}` was not found in this channel `{Context.Channel}`.");
            return;
        }

        if (text.Distinct().Count() != text.Length)
        {
            _logger.ZLogInformation($"Text `{text}` contains duplicate letters.");
            return;
        }

        var result = new List<Emoji>();
        foreach (var c in text)
        {
            var emoji = CommandConstants.EmojiChars?[c.ToString().ToUpper()];
            if (emoji == null)
            {
                _logger.ZLogInformation($"Text `{text}` contains non-emoji letters");
                return;
            }
            result.Add(emoji);
        }

        foreach (var emoji in result)
            await msg.AddReactionAsync(emoji);
    }
}