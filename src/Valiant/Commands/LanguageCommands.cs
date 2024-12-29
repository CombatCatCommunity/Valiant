using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LiteDB;

namespace Valiant.Commands;

[Group("languages")]
[RequireUserPermission(GuildPermission.Administrator)]
public class LanguageCommands : ModuleBase<SocketCommandContext>
{
    private readonly LiteDatabase _db = new("Filename=./data/languages.db;Connection=shared");

    [Command("all", Aliases = ["list"])]
    [Summary("List all languages supported by this instance of Valiant")]
    public async Task GetAllLanguagesAsync()
    {
        // List all cultures from ./locales

        await Context.Channel.SendMessageAsync("");
    }

    [Command("get", Aliases = ["current"])]
    [Summary("Get the currently configured language for this context")]
    public async Task GetLanguageAsync()
    {
        // Channel culture (if configured) and guild culture

        await Context.Channel.SendMessageAsync("");
    }

    [Command("set")]
    [Summary("Set the default language for the server or a specified channel")]
    public async Task SetLanguageAsync(string culture, SocketTextChannel channel = null)
    {
        // If channel is null, set guild culture

        await Context.Channel.SendMessageAsync("");
    }
}
