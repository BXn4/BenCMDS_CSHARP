using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord.WebSocket;
using System;
using System.Diagnostics;
using System.Net;
using Discord.Audio;
using System.Collections.Generic;

namespace BenCMDS
{
    public class parancsok : ModuleBase<SocketCommandContext>
    {
        string avatar = "";
        [Command("parancsok")]
        public async Task parancs()
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.AddField("A bankhoz tartozó parancsok", "**.bank**");
            eb.AddField("A titkosításhoz tartozó parancsok", "**.titkositas**");
            eb.AddField("Időjárás lekérdezés:", "**.idő {város}**");
            eb.AddField("Covid statisztika lekérdezés:", "**.covid {város}** (angolul)");
            eb.AddField("URL rövidítés:", "**.url {url}**");
            eb.AddField("Wikipédia oldal keresése:", "**.wiki {keresett oldal}**");
            eb.AddField("Wikipédia cikkek megjelenítése:", "**.swiki {keresett szó}**");
            eb.AddField("Fordítás:", "**.fordít {nyelv} szöveg... **(a nyelvet ISO 3166-1 alpha-2 formátumban kell megadni! pl.: hu, en, fr)\nhttps://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes");
            avatar = Context.Message.Author.GetAvatarUrl();
            eb.WithAuthor(Context.Message.Author.Username, avatar);
            eb.WithColor(Discord.Color.Blue);
            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }
        [Command("titkositas")]
        public async Task titkos()
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithTitle("A biztonság érdekében a BOT elküldi privátban a kulcsot!");
            eb.AddField(".hash gen", "Generál egy random kulcsot.");
            eb.AddField(".hash {kulcs}", "A kulcs helyére írd be a kulcsot.");
            eb.AddField(".ts {szöveg}", "Titkosítja a szöveget.");
            eb.AddField(".vts {szöveg}", "Dekódolja a szöveget.");
            eb.WithFooter(footer => footer.Text = "Csak akkor lesz jó, ha azonos kulcs van beállítva, mint amilyen kulccsal le lett titkosítva!");
            avatar = Context.Message.Author.GetAvatarUrl();
            eb.WithAuthor(Context.Message.Author.Username, avatar);
            eb.WithColor(Discord.Color.Blue);
            var builder = new ComponentBuilder().WithButton("Hash gen", "hashgen");
            await Context.Channel.SendMessageAsync("", false, eb.Build(), components: builder.Build());
            Context.Client.ButtonExecuted += gombpress;
        }
        [Command("info")]
        public async Task infok(SocketUser i)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.Title = $"{i.Username} információja:";
            eb.AddField("Csatlakozott:", i.CreatedAt);
            eb.AddField("Állapot:", i.Status);
            eb.AddField("ID:", i.Id);
            eb.AddField("Avatár ID:", i.AvatarId);
            eb.AddField("Név és hashtag:", i.Username + "#" + i.Discriminator);
            eb.AddField("Bot?", i.IsBot);
            eb.WithColor(Discord.Color.Blue);
            avatar = Context.Message.Author.GetAvatarUrl();
            eb.WithAuthor(avatar);
            eb.ThumbnailUrl = i.GetAvatarUrl();
            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }
        [Command("ping")]
        public async Task testx()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            WebClient wiki = new WebClient();
            var letoltes = wiki.DownloadString($"https://hu.wikipedia.org/w/api.php?action=opensearch&search=FACEBOOK");
            var jsonconvert = Newtonsoft.Json.JsonConvert.DeserializeObject(letoltes);
            stopwatch.Stop();
            double elapsed_time = stopwatch.ElapsedMilliseconds;
            await Context.Channel.SendMessageAsync($"PONG! {elapsed_time}ms");
        }
            public async Task gombpress(SocketMessageComponent arg)
        {
            switch (arg.Data.CustomId)
            {
                case "hashgen":
                    EmbedBuilder eb = new EmbedBuilder();
                    var betukszamokkarakterek = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789§'+!%/=()?:_~^|Ä€÷×äđĐ[]łŁ$ß¤<>#&@{}<*";
                    var stringbszk = new char[12];
                    var random = new Random();
                    for (int i = 0; i < stringbszk.Length; i++)
                    {
                        stringbszk[i] = betukszamokkarakterek[random.Next(betukszamokkarakterek.Length)];
                    }
                    var generated = new String(stringbszk);
                    eb.WithColor(Discord.Color.Green);
                    avatar = arg.User.GetAvatarUrl();
                    eb.WithAuthor(arg.User.Username, avatar);
                    eb.AddField("Sikrerült!", $"Az új generált kulcsod: **{generated}**\n A beállításhoz használd a: ```/hash {generated}``` parancsot!", false);
                    await arg.User.SendMessageAsync("", false, eb.Build());
                    break;
            }
        }
        [Command("kocka")]
        public async Task kockadob()
        {
            EmbedBuilder eb = new EmbedBuilder();
            Random random = new Random();
            int dobas = random.Next(1, 7);
            eb.WithColor(Discord.Color.Green);
            avatar = Context.User.GetAvatarUrl();
            eb.WithAuthor(Context.User.Username, avatar);
            eb.WithTitle($":game_die: A dobott számod: {dobas}");
            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }
        [Command("támogatás")]
        public async Task tamogat()
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithThumbnailUrl("https://media4.giphy.com/media/TDQOtnWgsBx99cNoyH/giphy.gif");
            eb.WithColor(Discord.Color.Gold);
            eb.AddField("Vegyél nekem egy sütit!", "Unalmamban szoktam csinálni egy két dolgot, amikkel általában megkönnyítem a dolgom, ha végzek vele. Amíg nincs kész, addig persze csak nehezítem a dolgaim :S.\n[Itt tudsz támogatni](https://www.buymeacoffee.com/bence912)!");
            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }
        [Command("kakilj")]
        public async Task kakilas()
        {
            EmbedBuilder eb = new EmbedBuilder();
            Random random = new Random();
            var kakik = new List<string>()
            {
                ":poop:",":poop: :poop:",":poop: :poop: :poop:",":poop: :poop: :poop: :poop:",":poop: :poop: :poop: :poop: :poop:",":poop: :poop: :poop: :poop: :poop: :poop:"
            };
            int kakiszam = random.Next(1, kakik.Count);
            eb.WithColor(Discord.Color.Green);
            avatar = Context.User.GetAvatarUrl();
            eb.WithAuthor(Context.User.Username, avatar);
            eb.AddField($"Ennyit kakiltam:", $"{kakik[kakiszam]}");
            await Context.Channel.SendMessageAsync("", false, eb.Build());
        } 
    }
}