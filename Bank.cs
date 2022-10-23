using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using System.IO;
using Discord.WebSocket;

namespace BenCMDS
{
    public class Bankcmd : ModuleBase<SocketCommandContext>
    {
        List<adatok> lista = new List<adatok>();
        string user = string.Empty;
        bool van = false;
        bool banknyitva = false;
        bool bankzárva = false;
        string avatar = "";
        [Command("bank")]
        public async Task bankinfo()
        {
            EmbedBuilder eb = new EmbedBuilder();
            avatar = Context.Message.Author.GetAvatarUrl();
            eb.WithAuthor(Context.Message.Author.Username, avatar);
            eb.WithColor(Discord.Color.Blue);
            eb.WithTitle("A bankhoz tartozó parancsok:");
            eb.AddField(".bank létrehoz", "Létrehoz egy bankot, ahol tudod tárolni a kreditjeid.", false);
            eb.AddField(".bank kinyit", "Kinyitod a fiókod, de vigyázz! Ilyenkor mások tudnak tőled kreditet rabolni!", false);
            eb.AddField(".bank bezár", "Bezárod a fiókod, így nem tudnak tőled rabolni, de nem férsz hozzá semmihez, amikez kreditek kellenek.", false);
            eb.AddField(".kredit @felhasználó", "fiókod, hogy a megemlített felhasználónak mennyi kredite van.\n*Ha a saját kreditjeid szeretnéd lekérdezni, akkor csak a parancsot írd be. (.kredit)*", false);
            eb.AddField(".küld @felhasználó {kreditek száma}", "Az megemlített felhasználónak tudsz küldeni kreditet.", false);
            eb.AddField(".rabol @felhasználó", "A megemlített felhasználótol tudsz egy kevés kreditet elrabolni.", false);
            eb.AddField(".statisztika @felhasználó", "Kilistáza a megemlített felhasználó statisztikáját.", false);
            eb.AddField(".bank töröl", "A fiókod megszünteted, ezzel a krediteid is elvesznek.", false);
            eb.WithAuthor(Context.Message.Author.Username);
            var builder = new ComponentBuilder().WithButton("Létrehoz", "letrehoz").WithButton("Kinyit","kinyit")
            .WithButton("Bezár","bezar").WithButton("Kredit","kredit").WithButton("Statisztika","statisztika");
            await Context.Channel.SendMessageAsync("", false, eb.Build(),components: builder.Build());
            Context.Client.ButtonExecuted += gombpress;
        }
        public async Task gombpress(SocketMessageComponent arg)
        {
            switch (arg.Data.CustomId)
            {
                case "letrehoz":
                    await arg.RespondAsync($"{arg.User.Mention}");
                    //await bankletrehoz();
                    EmbedBuilder eb = new EmbedBuilder();
                    string path = $@"Bankok\{arg.User.Id}bankja.txt";
                    van = File.Exists(path);
                    if (van == true)
                    {
                        eb.AddField("Neked van már fiókod!", "Ha törölni szeretnéd a fiókod, akkor használd a **.bank töröl** parancsot.");
                        avatar = arg.User.GetAvatarUrl();
                        eb.WithAuthor(arg.User.Username, avatar);
                        eb.WithColor(Discord.Color.Orange);
                        await arg.Channel.SendMessageAsync("", false, eb.Build());
                    }
                    else
                    {
                        File.WriteAllText($@"Bankok\{arg.User.Id}bankja.txt", $"{arg.User.Id};1000;zárva");
                        eb.AddField("Sikeresen elkészítettem a fiókod!", "\nKrediteid száma: **1000** :coin: \nFiókod állapota: **Zárva**\nA további parancsokért használd a **.bank** parancsot.");
                        eb.WithColor(Discord.Color.Green);
                        avatar = arg.User.GetAvatarUrl();
                        eb.WithAuthor(arg.User.Username, avatar);
                        await arg.Channel.SendMessageAsync("", false, eb.Build());
                    }
                    lista.Clear();
                    break;
                case "kinyit":
                    await arg.RespondAsync($"{arg.User.Mention}");
                    path = $@"Bankok\{arg.User.Id}bankja.txt";
                    van = File.Exists(path);
                    EmbedBuilder ebki = new EmbedBuilder();
                    if (van == true)
                    {
                        foreach (var item in File.ReadAllLines($@"Bankok\{arg.User.Id}bankja.txt"))
                        {
                            adatok Adat = new adatok(item);
                            lista.Add(Adat);
                        }
                        foreach (var item in lista)
                        {
                            if (item.allapot == "zárva")
                            {
                                banknyitva = false;
                            }
                            if (item.allapot == "nyitva")
                            {
                                banknyitva = true;
                            }
                            break;
                        }
                        if (banknyitva == false)
                        {
                            string text = File.ReadAllText($@"Bankok\{arg.User.Id}bankja.txt");
                            text = text.Replace("zárva", "nyitva");
                            File.WriteAllText($@"Bankok\{arg.User.Id}bankja.txt", text);
                            ebki.AddField("Sikrerült!", "Sikeresen kinyitotad a fiókod.\nA fiókod állapota: **Nyitva**", false);
                            ebki.WithColor(Discord.Color.Green);
                            avatar = arg.User.GetAvatarUrl();
                            ebki.WithAuthor(arg.User.Username, avatar);
                            await arg.Channel.SendMessageAsync("", false, ebki.Build());
                        }
                        else
                        {
                            ebki.AddField("Hiba!", "A fiókod jelenleg is nyitva van.\nHa szeretnéd bezárni, akkor használd a **.bank bezár** parancsot.", false);
                            ebki.WithColor(Discord.Color.Red);
                            avatar = arg.User.GetAvatarUrl();
                            ebki.WithAuthor(arg.User.Username, avatar);
                            await arg.Channel.SendMessageAsync("", false, ebki.Build());
                        }
                    }
                    else
                    {
                        ebki.AddField("Hiba!", "Neked még nincs fiókod!\nHasználd a **.bank létrehoz** parancsot, hogy készíts egyet.", false);
                        ebki.WithColor(Discord.Color.Red);
                        avatar = arg.Message.Author.GetAvatarUrl();
                        ebki.WithAuthor(arg.User.Username, avatar);
                        await arg.Channel.SendMessageAsync("", false, ebki.Build());
                    }
                    lista.Clear();
                    break;
                case "bezar":
                    EmbedBuilder ebbez = new EmbedBuilder();
                    await arg.RespondAsync($"{arg.User.Mention}");
                    path = $@"Bankok\{arg.User.Id}bankja.txt";
                    van = File.Exists(path);
                    if (van == true)
                    {
                        foreach (var item in File.ReadAllLines($@"Bankok\{arg.User.Id}bankja.txt"))
                        {
                            adatok Adat = new adatok(item);
                            lista.Add(Adat);
                        }
                        foreach (var item in lista)
                        {
                            if (item.allapot == "nyitva")
                            {
                                bankzárva = false;
                            }
                            if (item.allapot == "zárva")
                            {
                                bankzárva = true;
                            }
                            break;
                        }
                        if (bankzárva == false)
                        {
                            string text = File.ReadAllText($@"Bankok\{arg.User.Id}bankja.txt");
                            text = text.Replace("nyitva", "zárva");
                            File.WriteAllText($@"Bankok\{arg.User.Id}bankja.txt", text);
                            ebbez.AddField("Sikrerült!", "Sikeresen bezártad a fiókod.\nA fiókod állapota: **Zárva**", false);
                            ebbez.WithColor(Discord.Color.Green);
                            avatar = arg.User.GetAvatarUrl();
                            ebbez.WithAuthor(arg.User.Username, avatar);
                            await arg.Channel.SendMessageAsync("", false, ebbez.Build());
                        }
                        else
                        {
                            ebbez.AddField("Hiba!", "A fiókod jelenleg is zárva van.\nHa szeretnéd kinyitni, akkor használd a **.bank kinyit** parancsot.", false);
                            ebbez.WithColor(Discord.Color.Red);
                            avatar = arg.User.GetAvatarUrl();
                            ebbez.WithAuthor(arg.User.Username, avatar);
                            await arg.Channel.SendMessageAsync("", false, ebbez.Build());
                        }
                    }
                    else
                    {
                        ebbez.AddField("Hiba!", "Neked még nincs fiókod!\nHasználd a **.bank létrehoz** parancsot, hogy készíts egyet.", false);
                        ebbez.WithColor(Discord.Color.Red);
                        avatar = arg.User.GetAvatarUrl();
                        ebbez.WithAuthor(arg.User.Username, avatar);
                        await arg.Channel.SendMessageAsync("", false, ebbez.Build());
                    }
                    lista.Clear();
                    break;
                case "kredit":
                    await arg.RespondAsync($"{arg.User.Mention}");
                    path = $@"Bankok\{arg.User.Id}bankja.txt";
                    van = File.Exists(path);
                    if (van == true)
                    {
                        EmbedBuilder ebk = new EmbedBuilder();
                        foreach (var item in File.ReadAllLines($@"Bankok\{arg.User.Id}bankja.txt"))
                        {
                            adatok Adat = new adatok(item);
                            lista.Add(Adat);
                        }
                        foreach (var item in lista)
                        {
                            int kreditek = item.penz;
                            ebk.AddField("Kredit lekérdezés:", $"Neked összesen **{kreditek}** kredited van.");
                            ebk.WithColor(Discord.Color.Blue);
                            avatar = arg.User.GetAvatarUrl();
                            ebk.WithAuthor(arg.User.Username, avatar);
                            await arg.Channel.SendMessageAsync("", false, ebk.Build());
                        }
                    }
                    break;
                case "statisztika":
                    await arg.RespondAsync($"{Context.User.Mention}");
                    await Context.Channel.SendMessageAsync("Még nincs kész!");
                    break;

            }
        }
        [Command("bank létrehoz")]
        public async Task bankletrehoz()
        {
            EmbedBuilder eb = new EmbedBuilder();
            string path = $@"Bankok\{Context.User.Id}bankja.txt";
            van = File.Exists(path);
            if (van == true)
            {   
                eb.AddField("Neked van már fiókod!", "Ha törölni szeretnéd a fiókod, akkor használd a **.bank töröl** parancsot.");
                avatar = Context.Message.Author.GetAvatarUrl();
                eb.WithAuthor(Context.Message.Author.Username, avatar);
                eb.WithColor(Discord.Color.Orange);
                await Context.Channel.SendMessageAsync("", false, eb.Build());
            }
            else
            {
                File.WriteAllText($@"Bankok\{Context.User.Id}bankja.txt", $"{Context.User.Id};1000;zárva");
                eb.AddField("Sikeresen elkészítettem a fiókod!", "\nKrediteid száma: **1000**\nFiókod állapota: **Zárva**\nA további parancsokért használd a **.bank** parancsot.");
                eb.WithColor(Discord.Color.Green);
                avatar = Context.Message.Author.GetAvatarUrl();
                eb.WithAuthor(Context.Message.Author.Username, avatar);
                await Context.Channel.SendMessageAsync("", false, eb.Build());
            }
            lista.Clear(); 
        }
        [Command("bank kinyit")]
        public async Task banknyit()
        {
            EmbedBuilder eb = new EmbedBuilder();
            string path = $@"Bankok\{Context.User.Id}bankja.txt";
            van = File.Exists(path);
            if (van == true)
            {
                foreach (var item in File.ReadAllLines($@"Bankok\{Context.User.Id}bankja.txt"))
                {
                    adatok Adat = new adatok(item);
                    lista.Add(Adat);
                }
                foreach (var item in lista)
                {
                    if(item.allapot == "zárva")
                    {
                        banknyitva = false;
                    }
                    if (item.allapot == "nyitva")
                    {
                        banknyitva = true;
                    }
                    break;
                }
                if (banknyitva == false)
                {
                    string text = File.ReadAllText($@"Bankok\{Context.User.Id}bankja.txt");
                    text = text.Replace("zárva", "nyitva");
                    File.WriteAllText($@"Bankok\{Context.User.Id}bankja.txt", text);
                    eb.AddField("Sikrerült!", "Sikeresen kinyitotad a fiókod.\nA fiókod állapota: **Nyitva**", false);
                    eb.WithColor(Discord.Color.Green);
                    avatar = Context.Message.Author.GetAvatarUrl();
                    eb.WithAuthor(Context.Message.Author.Username, avatar);
                    await Context.Channel.SendMessageAsync("", false, eb.Build());
                }
                else
                {
                    eb.AddField("Hiba!", "A fiókod jelenleg is nyitva van.\nHa szeretnéd bezárni, akkor használd a **.bank bezár** parancsot.", false);
                    eb.WithColor(Discord.Color.Red);
                    avatar = Context.Message.Author.GetAvatarUrl();
                    eb.WithAuthor(Context.Message.Author.Username, avatar);
                    await Context.Channel.SendMessageAsync("", false, eb.Build());
                } 
            }
            else
            {
                eb.AddField("Hiba!", "Neked még nincs fiókod!\nHasználd a **.bank létrehoz** parancsot, hogy készíts egyet.", false);
                eb.WithColor(Discord.Color.Red);
                avatar = Context.Message.Author.GetAvatarUrl();
                eb.WithAuthor(Context.Message.Author.Username, avatar);
                await Context.Channel.SendMessageAsync("", false, eb.Build());
            }
            lista.Clear();
        }
        [Command("bank bezár")]
        public async Task bankbezar()
        {
            EmbedBuilder eb = new EmbedBuilder();
            string path = $@"Bankok\{Context.User.Id}bankja.txt";
            van = File.Exists(path);
            if (van == true)
            {
                foreach (var item in File.ReadAllLines($@"Bankok\{Context.User.Id}bankja.txt"))
                {
                    adatok Adat = new adatok(item);
                    lista.Add(Adat);
                }
                foreach (var item in lista)
                {
                    if (item.allapot == "nyitva")
                    {
                        bankzárva = false;
                    }
                    if (item.allapot == "zárva")
                    {
                        bankzárva = true;
                    }
                    break;
                }
                if (bankzárva == false)
                {
                    string text = File.ReadAllText($@"Bankok\{Context.User.Id}bankja.txt");
                    text = text.Replace("nyitva", "zárva");
                    File.WriteAllText($@"Bankok\{Context.User.Id}bankja.txt", text);
                    eb.AddField("Sikrerült!", "Sikeresen bezártad a fiókod.\nA fiókod állapota: **Zárva**", false);
                    eb.WithColor(Discord.Color.Green);
                    avatar = Context.Message.Author.GetAvatarUrl();
                    eb.WithAuthor(Context.Message.Author.Username, avatar);
                    await Context.Channel.SendMessageAsync("", false, eb.Build());
                }
                else
                {
                    eb.AddField("Hiba!", "A fiókod jelenleg is zárva van.\nHa szeretnéd kinyitni, akkor használd a **.bank kinyit** parancsot.", false);
                    eb.WithColor(Discord.Color.Red);
                    avatar = Context.Message.Author.GetAvatarUrl();
                    eb.WithAuthor(Context.Message.Author.Username, avatar);
                    await Context.Channel.SendMessageAsync("", false, eb.Build());
                }
            }
            else
            {
                eb.AddField("Hiba!", "Neked még nincs fiókod!\nHasználd a **.bank létrehoz** parancsot, hogy készíts egyet.", false);
                eb.WithColor(Discord.Color.Red);
                avatar = Context.Message.Author.GetAvatarUrl();
                eb.WithAuthor(Context.Message.Author.Username, avatar);
                await Context.Channel.SendMessageAsync("", false, eb.Build());
            }
            lista.Clear();
        }
        [Command("bank töröl")]
        public async Task banktorol()
        {
            EmbedBuilder eb = new EmbedBuilder();
            user = Context.Message.Author.Mention;
            foreach (var item in File.ReadAllLines(@"Bankok\!bankoktörlése.txt"))
            {
                adatok Adat = new adatok(item);
                lista.Add(Adat);
            }
            foreach (var item in lista)
            {
                if (item.nev == user)
                {
                    van = true;
                }

            }
            if (van == true)
            {
                eb.AddField("Hiba!", "Neked már van törlési kérelmed!\nHa megszeretnéd szakítani a folyamatot, akkor írj rá Bencére! (bence#0123)", false);
                eb.WithColor(Discord.Color.Red);
                avatar = Context.Message.Author.GetAvatarUrl();
                eb.WithAuthor(Context.Message.Author.Username, avatar);
                await Context.Channel.SendMessageAsync("", false, eb.Build());
                lista.Clear();
            }
            else
            {
                File.AppendAllText(@"Bankok\!bankoktörlése.txt", $"\n{Context.Message.Author.Mention};0;zárva");
                eb.AddField("Sikrerült!", "Sikeresen továbítottam a törlési kérelmed, hamarosan a fiókod véglegesen törölve lesz!\nHa megszeretnéd szakítani a folyamatot, akkor írj rá Bencére! (bence#0123)", false);
                eb.WithColor(Discord.Color.Green);
                avatar = Context.Message.Author.GetAvatarUrl();
                eb.WithAuthor(Context.Message.Author.Username, avatar);
                await Context.Channel.SendMessageAsync("", false, eb.Build());
                lista.Clear();
            }
            lista.Clear();
            }
    }
}
