using Discord;
using Discord.Interactions;

namespace Valiant.Interactions;

[RequireUserPermission(GuildPermission.Administrator)]
public class AdminModule : InteractionModuleBase<SocketInteractionContext>
{
    private const ulong LogChannelId = 1209800311234105344;

    [SlashCommand("sendmessage", "Make the bot send a message")]
    public async Task SendMessageAsync(string content, ITextChannel channel = null, ulong? replyMsgId = null)
    {
        MessageReference replyTo = null;
        if (replyMsgId != null)
            replyTo = new(replyMsgId);

        IUserMessage sentmsg = channel == null
            ? await Context.Channel.SendMessageAsync(content, messageReference: replyTo)
            : await channel.SendMessageAsync(content, messageReference: replyTo);

        await RespondAsync($"Message sent, see {sentmsg.GetJumpUrl()}", ephemeral: true);

        var logTo = Context.Guild.GetTextChannel(LogChannelId);
        if (logTo == null)
            return;

        var embed = new EmbedBuilder()
            .WithColor(Color.Blue)
            .WithTitle("Message sent using command")
            .WithDescription($"> **Message Id:** `{sentmsg.Id}`" +
                             $"\n> **Channel:** {(sentmsg.Channel as ITextChannel).Mention}" +
                             $"\n> [URL]({sentmsg.GetJumpUrl()})")
            .AddField("Message", content)
            .WithFooter($"@{Context.User.GlobalName} ({Context.User.Id})", Context.User.GetDisplayAvatarUrl())
            .WithTimestamp(sentmsg.Timestamp);

        await logTo.SendMessageAsync(embed: embed.Build());
    }
}
