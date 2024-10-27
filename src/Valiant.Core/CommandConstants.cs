using Discord;

namespace Valiant;
public static class CommandConstants
{
    public static readonly Dictionary<string, Emoji> EmojiChars;

    static CommandConstants()
    {
        EmojiChars = new()
        {
            { "A", new Emoji("🇦") },
            { "B", new Emoji("🇧") },
            { "C", new Emoji("🇨") },
            { "D", new Emoji("🇩") },
            { "E", new Emoji("🇪") },
            { "F", new Emoji("🇫") },
            { "G", new Emoji("🇬") },
            { "H", new Emoji("🇭") },
            { "I", new Emoji("🇮") },
            { "J", new Emoji("🇯") },
            { "K", new Emoji("🇰") },
            { "L", new Emoji("🇱") },
            { "M", new Emoji("🇲") },
            { "N", new Emoji("🇳") },
            { "O", new Emoji("🇴") },
            { "P", new Emoji("🇵") },
            { "Q", new Emoji("🇶") },
            { "R", new Emoji("🇷") },
            { "S", new Emoji("🇸") },
            { "T", new Emoji("🇹") },
            { "U", new Emoji("🇺") },
            { "V", new Emoji("🇻") },
            { "W", new Emoji("🇼") },
            { "X", new Emoji("🇽") },
            { "Y", new Emoji("🇾") },
            { "Z", new Emoji("🇿") },
            { "0", new Emoji("0️⃣") },
            { "1", new Emoji("1️⃣") },
            { "2", new Emoji("2️⃣") },
            { "3", new Emoji("3️⃣") },
            { "4", new Emoji("4️⃣") },
            { "5", new Emoji("5️⃣") },
            { "6", new Emoji("6️⃣") },
            { "7", new Emoji("7️⃣") },
            { "8", new Emoji("8️⃣") },
            { "9", new Emoji("️9️⃣") }
        };
    }
}
