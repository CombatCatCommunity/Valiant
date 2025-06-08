using Discord;
using Discord.Commands;

namespace Valiant.Commands;

public class WhateverCommands : ModuleBase<SocketCommandContext>
{
    //[Command("fixforums")]
    //public async Task FixForumsAsync()
    //{
    //    var forum = Context.Guild.GetForumChannel(1210163526753194017);

    //    var active = await forum.GetActiveThreadsAsync();
    //    foreach (var thread in active)
    //    {
    //        await thread.ModifyAsync(x =>
    //        {
    //            x.SlowModeInterval = (int)TimeSpan.FromHours(1).TotalSeconds;
    //        });
    //    }

    //    var reply = new MessageReference(Context.Message.Id);
    //    await ReplyAsync($"Fixed the slowmode for {active.Count} active threads in {forum.Mention}", messageReference: reply);
    //}

    [Command("aprilfools")]
    public async Task AprilFoolsAsync()
    {
        await Context.Guild.GetCategoryChannel(1277464813441646682).ModifyAsync(x => x.Name = @"OℲNI");
        await Context.Guild.GetChannel(1240493902256013363).ModifyAsync(x => x.Name = @"ǝɯoɔlǝʍ🎉");
        await Context.Guild.GetChannel(1244671186663899259).ModifyAsync(x => x.Name = @"ʎɟᴉɹǝʌ✅");
        await Context.Guild.GetChannel(1236178515825659946).ModifyAsync(x => x.Name = @"bɐɟ❓");

        await Context.Guild.GetCategoryChannel(1235474567389249627).ModifyAsync(x => x.Name = @"SMƎN");
        await Context.Guild.GetChannel(1209807246930743336).ModifyAsync(x => x.Name = @"sʍǝu-ǝɯɐƃ📢");
        await Context.Guild.GetChannel(1209808191454445578).ModifyAsync(x => x.Name = @"sʍǝu-ʇɐɔʇɐqɯoɔ📢");
        await Context.Guild.GetChannel(1235464626569740371).ModifyAsync(x => x.Name = @"sʍǝu-ʎʇᴉunɯɯoɔ📢");
        await Context.Guild.GetChannel(1209809519404777492).ModifyAsync(x => x.Name = @"ʇsᴉl-uɐq-ɹǝʞɔɐɥ🚫");

        await Context.Guild.GetCategoryChannel(1209810824894615552).ModifyAsync(x => x.Name = @"┴∩Oq∀");
        await Context.Guild.GetChannel(1209812456591855677).ModifyAsync(x => x.Name = @"ʇɐɔʇɐqɯoɔ-ʇnoqɐ😸");
        await Context.Guild.GetChannel(1209810908088635433).ModifyAsync(x => x.Name = @"ʇlnɐssɐ-plᴉʍ🐾");
        await Context.Guild.GetChannel(1209811948070379520).ModifyAsync(x => x.Name = @"oɹʇuᴉ-ʇuɐᴉlɐʌ🎖");
        await Context.Guild.GetChannel(1209812291147538482).ModifyAsync(x => x.Name = @"oɹʇuᴉ-sǝpoɯ🕹");
        await Context.Guild.GetChannel(1351120392466464872).ModifyAsync(x => x.Name = @"ʇᴉʞ-ɐᴉpǝɯ🖼");

        await Context.Guild.GetCategoryChannel(1351461131247685632).ModifyAsync(x => x.Name = @"┴NƎΛƎ ⅄┴IN∩WWOƆ🥳");
        await Context.Guild.GetChannel(1351463454690770975).ModifyAsync(x => x.Name = @"ʇuǝʌǝ-ʇuǝɯoɯ-ɐʍ");

        await Context.Guild.GetCategoryChannel(1210154069302448138).ModifyAsync(x => x.Name = @"🔴⅄┴IN∩WWOƆ🔴");
        await Context.Guild.GetChannel(1235441556358692864).ModifyAsync(x => x.Name = @"ɔᴉdoʇ-ɟɟo🍻");
        await Context.Guild.GetChannel(1210154766622130227).ModifyAsync(x => x.Name = @"uǝ-ʇɐɥɔ-ǝɯɐƃ💬");
        await Context.Guild.GetChannel(1235566934058209351).ModifyAsync(x => x.Name = @"sǝ-ʇɐɥɔ-ǝɯɐƃ💬");
        await Context.Guild.GetChannel(1235568362646536276).ModifyAsync(x => x.Name = @"ɹɟ-ʇɐɥɔ-ǝɯɐƃ💬");
        await Context.Guild.GetChannel(1281435145014083667).ModifyAsync(x => x.Name = @"ʇd-ʇɐɥɔ-ǝɯɐƃ💬");
        await Context.Guild.GetChannel(1235565857938345994).ModifyAsync(x => x.Name = @"nɹ-ʇɐɥɔ-ǝɯɐƃ💬");
        await Context.Guild.GetChannel(1210156557921161246).ModifyAsync(x => x.Name = @"ɥz-ʇɐɥɔ-ǝɯɐƃ💬");
        await Context.Guild.GetChannel(1235454325606715433).ModifyAsync(x => x.Name = @"ʇɐɥɔ-sʇɐɹʇs⚔");
        await Context.Guild.GetChannel(1349525183115038834).ModifyAsync(x => x.Name = @"ʇɐɥɔ-ᴉʞᴉʍ💡");
        await Context.Guild.GetChannel(1210155193023336480).ModifyAsync(x => x.Name = @"ʇɐɥɔ-oɹɥʇuɐ🦊");
        await Context.Guild.GetChannel(1210157680598056970).ModifyAsync(x => x.Name = @"sɹoʇɐǝɹɔ⭐");

        await Context.Guild.GetCategoryChannel(1210156967197282344).ModifyAsync(x => x.Name = @"Ǝɹ∀HS🎨");
        await Context.Guild.GetChannel(1216564615861895311).ModifyAsync(x => x.Name = @"ʇɹɐ-uɐɟ🎨");
        await Context.Guild.GetChannel(1210157515879223367).ModifyAsync(x => x.Name = @"sʇoɥsuǝǝɹɔs-sdᴉlɔ🎬");
        await Context.Guild.GetChannel(1235457459418697748).ModifyAsync(x => x.Name = @"ɐᴉpǝɯ📱");
        await Context.Guild.GetChannel(1282088409409519738).ModifyAsync(x => x.Name = @"ɔᴉsnɯ🎵");
        await Context.Guild.GetChannel(1210158154986422282).ModifyAsync(x => x.Name = @"sǝɯǝɯ🤡");
        await Context.Guild.GetChannel(1210158478161616907).ModifyAsync(x => x.Name = @"uoᴉʇoɯoɹd-ɟlǝs🎙");

        await Context.Guild.GetCategoryChannel(1235432840188137472).ModifyAsync(x => x.Name = @"┴ɹOԀԀ∩S");
        await Context.Guild.GetChannel(1210163526753194017).ModifyAsync(x => x.Name = @"suoᴉʇsǝƃƃns-ǝɯɐƃ❤");
        await Context.Guild.GetChannel(1210160013348175882).ModifyAsync(x => x.Name = @"suoᴉʇsǝƃƃns-pɹoɔsᴉp🗨");

        await Context.Guild.GetCategoryChannel(1210166872977317918).ModifyAsync(x => x.Name = @"⅄∀˥Ԁ");
        await Context.Guild.GetChannel(1235445037379358730).ModifyAsync(x => x.Name = @"ƃɟl🚩");
        await Context.Guild.GetChannel(1235785910378037299).ModifyAsync(x => x.Name = @"sɔʌ-ʇnoqɐ");
        await Context.Guild.GetChannel(1350659095123984486).ModifyAsync(x => x.Name = @"ƆΛ ǝʇɐǝɹƆ➕");

        await Context.Guild.GetCategoryChannel(1247749541973983283).ModifyAsync(x => x.Name = @"SפO˥");
        await Context.Guild.GetChannel(1247749612048220202).ModifyAsync(x => x.Name = @"ƃol-poɯ🛡");
    }

    [Command("unaprilfools")]
    public async Task UnaprilFoolsAsync()
    {
        await Context.Guild.GetCategoryChannel(1277464813441646682).ModifyAsync(x => x.Name = @"INFO");
        await Context.Guild.GetChannel(1240493902256013363).ModifyAsync(x => x.Name = @"🎉welcome");
        await Context.Guild.GetChannel(1244671186663899259).ModifyAsync(x => x.Name = @"✅verify");
        await Context.Guild.GetChannel(1236178515825659946).ModifyAsync(x => x.Name = @"❓faq");

        await Context.Guild.GetCategoryChannel(1235474567389249627).ModifyAsync(x => x.Name = @"NEWS");
        await Context.Guild.GetChannel(1209807246930743336).ModifyAsync(x => x.Name = @"📢game-news");
        await Context.Guild.GetChannel(1209808191454445578).ModifyAsync(x => x.Name = @"📢combatcat-news");
        await Context.Guild.GetChannel(1235464626569740371).ModifyAsync(x => x.Name = @"📢community-news");
        await Context.Guild.GetChannel(1209809519404777492).ModifyAsync(x => x.Name = @"🚫hacker-ban-list");

        await Context.Guild.GetCategoryChannel(1209810824894615552).ModifyAsync(x => x.Name = @"ABOUT");
        await Context.Guild.GetChannel(1209812456591855677).ModifyAsync(x => x.Name = @"😸about-combatcat");
        await Context.Guild.GetChannel(1209810908088635433).ModifyAsync(x => x.Name = @"🐾wild-assault");
        await Context.Guild.GetChannel(1209811948070379520).ModifyAsync(x => x.Name = @"🎖valiant-intro");
        await Context.Guild.GetChannel(1209812291147538482).ModifyAsync(x => x.Name = @"🕹modes-intro");
        await Context.Guild.GetChannel(1351120392466464872).ModifyAsync(x => x.Name = @"🖼media-kit");

        await Context.Guild.GetCategoryChannel(1351461131247685632).ModifyAsync(x => x.Name = @"🥳COMMUNITY EVENT");
        await Context.Guild.GetChannel(1351463454690770975).ModifyAsync(x => x.Name = @"wa-moment-event");

        await Context.Guild.GetCategoryChannel(1210154069302448138).ModifyAsync(x => x.Name = @"🔴COMMUNITY🔴");
        await Context.Guild.GetChannel(1235441556358692864).ModifyAsync(x => x.Name = @"🍻off-topic");
        await Context.Guild.GetChannel(1210154766622130227).ModifyAsync(x => x.Name = @"💬game-chat-en");
        await Context.Guild.GetChannel(1235566934058209351).ModifyAsync(x => x.Name = @"💬game-chat-es");
        await Context.Guild.GetChannel(1235568362646536276).ModifyAsync(x => x.Name = @"💬game-chat-fr");
        await Context.Guild.GetChannel(1281435145014083667).ModifyAsync(x => x.Name = @"💬game-chat-pt");
        await Context.Guild.GetChannel(1235565857938345994).ModifyAsync(x => x.Name = @"💬game-chat-ru");
        await Context.Guild.GetChannel(1210156557921161246).ModifyAsync(x => x.Name = @"💬game-chat-zh");
        await Context.Guild.GetChannel(1235454325606715433).ModifyAsync(x => x.Name = @"⚔strats-chat");
        await Context.Guild.GetChannel(1349525183115038834).ModifyAsync(x => x.Name = @"💡wiki-chat");
        await Context.Guild.GetChannel(1210155193023336480).ModifyAsync(x => x.Name = @"🦊anthro-chat");
        await Context.Guild.GetChannel(1210157680598056970).ModifyAsync(x => x.Name = @"⭐creators");

        await Context.Guild.GetCategoryChannel(1210156967197282344).ModifyAsync(x => x.Name = @"🎨SHARE");
        await Context.Guild.GetChannel(1216564615861895311).ModifyAsync(x => x.Name = @"🎨fan-art");
        await Context.Guild.GetChannel(1210157515879223367).ModifyAsync(x => x.Name = @"🎬clips-screenshots");
        await Context.Guild.GetChannel(1235457459418697748).ModifyAsync(x => x.Name = @"📱media");
        await Context.Guild.GetChannel(1282088409409519738).ModifyAsync(x => x.Name = @"🎵music");
        await Context.Guild.GetChannel(1210158154986422282).ModifyAsync(x => x.Name = @"🤡memes");
        await Context.Guild.GetChannel(1210158478161616907).ModifyAsync(x => x.Name = @"🎙self-promo");

        await Context.Guild.GetCategoryChannel(1235432840188137472).ModifyAsync(x => x.Name = @"SUPPORT");
        await Context.Guild.GetChannel(1210163526753194017).ModifyAsync(x => x.Name = @"❤game-suggestions");
        await Context.Guild.GetChannel(1210160013348175882).ModifyAsync(x => x.Name = @"🗨discord-suggestions");

        await Context.Guild.GetCategoryChannel(1210166872977317918).ModifyAsync(x => x.Name = @"PLAY");
        await Context.Guild.GetChannel(1235445037379358730).ModifyAsync(x => x.Name = @"🚩lfg");
        await Context.Guild.GetChannel(1235785910378037299).ModifyAsync(x => x.Name = @"about-vcs");
        await Context.Guild.GetChannel(1350659095123984486).ModifyAsync(x => x.Name = @"➕Create VC");

        await Context.Guild.GetCategoryChannel(1247749541973983283).ModifyAsync(x => x.Name = @"LOGS");
        await Context.Guild.GetChannel(1236178515825659946).ModifyAsync(x => x.Name = @"🛡mod-log");
    }
}
