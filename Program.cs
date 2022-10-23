using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Timers;
using System.Security.Cryptography;
using System.Net;
using RestSharp;
using System.Text.RegularExpressions;
using System.Linq;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace BenCMDS
{
    class Program
    {
        static void Main(string[] args) => new Program().bot_indit().GetAwaiter().GetResult();
        private DiscordSocketClient kliens;
        private CommandService parancsok;
        private IServiceProvider szolgaltatasok;
        public string prefix = string.Empty;
        public string token = string.Empty;
        public string botneve = string.Empty;
        List<adatok> lista = new List<adatok>();
        List<titkositasok> hashlista = new List<titkositasok>();
        List<rablasok> rablaslista = new List<rablasok>();
        List<blackjack> bjlista = new List<blackjack>();
        int kreditek = 0;
        int i = 0;
        private List<string> statuslista = new List<string>() { ".parancsok" };
        public struct config
        {
            [JsonProperty("token")]
            public string Token { get; set; }
            [JsonProperty("prefix")]
            public string Prefix { get; set; }
            [JsonProperty("botneve")]
            public string Botneve { get; set; }
        }
        public async Task bot_indit()
        {
            var json = string.Empty;
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);
            var configjson = JsonConvert.DeserializeObject<config>(json);
            kliens = new DiscordSocketClient();
            parancsok = new CommandService();
            szolgaltatasok = new ServiceCollection()
                 .AddSingleton(kliens)
                .AddSingleton(new AudioService())
                .AddSingleton(parancsok).BuildServiceProvider();
            await kliens.SetStatusAsync(UserStatus.Online);
            await kliens.SetGameAsync("Elakadtál? .parancsok", "", ActivityType.Watching);
            System.Timers.Timer statusztimer = new System.Timers.Timer();
            statusztimer.Elapsed += new ElapsedEventHandler(statusztimertick);
            statusztimer.Interval = 10000;
            statusztimer.Enabled = true;
            var config = new config()
            {
                Token = configjson.Token,
                Prefix = configjson.Prefix,
                Botneve = configjson.Botneve

            };
            prefix = config.Prefix;
            token = config.Token;
            botneve = config.Botneve;
            Console.WriteLine($"A {botneve} éppen indul. A parancsok használatához használd a: {prefix} karaktert a parancs elején.");
            //Console.ReadKey();
            kliens.Log += kliens_Log;
            await parancsokregisztral();
            await kliens.LoginAsync(TokenType.Bot, token);
            await kliens.StartAsync();
            await Task.Delay(-1);
        }
        private Task kliens_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task parancsokregisztral()
        {
            kliens.MessageReceived += parancsasyncs;
            await parancsok.AddModulesAsync(Assembly.GetEntryAssembly(), szolgaltatasok);
        }
        private async Task parancsasyncs(SocketMessage arg)
        {
                //await Task.Run(async () => {
                var uzenet = arg as SocketUserMessage;
                var context = new SocketCommandContext(kliens, uzenet);
                if (uzenet.Author.IsBot)
                {
                    return;
                }
                int argx = 0;
                if (uzenet.HasStringPrefix(prefix, ref argx))
                {
                    var eredmeny = await parancsok.ExecuteAsync(context, argx, szolgaltatasok);
                    if (!eredmeny.IsSuccess) Console.WriteLine(eredmeny.ErrorReason);
                }
                string szoveg = context.Message.Content;
                string masodikresz = context.Message.Content;
                string ertek = context.Message.Content;
                string[] tomb = szoveg.Split(' ');
                string parancs = tomb[0];
                var replacement = "";
                var replacemente = "";
                var replacementk = "";
                string user = "";
                string avatar = "";
                bool van = false;
                bool vanilyenfelhasznalo = false;
                int emlitettpenze = 0;
                int sajatpenz = 0;
                bool sajatzarva = false;
                bool emlitettzarva = false;
                ///////////////////////////////////
                ///////////////////////////////////
                #region KAROMKODAS SZURO (58) KIKAPCSOLVA!
                /*          if (szoveg.Contains("anyád") || szoveg.Contains("buzi") || szoveg.Contains("fasz") || szoveg.Contains("geci")
               || szoveg.Contains("kurva") || szoveg.Contains("pöcs") || szoveg.Contains("pénisz") || szoveg.Contains("pina")
               || szoveg.Contains("punci") || szoveg.Contains("gyökér") || szoveg.Contains("fasszopó")
               || szoveg.Contains("fasszopo") || szoveg.Contains("penisz") || szoveg.Contains("pocs")
               || szoveg.Contains("gyoker") || szoveg.Contains("sperma") || szoveg.Contains("szex") || szoveg.Contains("szopás")
               || szoveg.Contains("szopas") || szoveg.Contains("farok") || szoveg.Contains("cici")
               || szoveg.Contains("idióta") || szoveg.Contains("idiota") || szoveg.Contains("retardált")
               || szoveg.Contains("retardalt") || szoveg.Contains("bazdmeg")
               || szoveg.Contains("basszátokmeg") || szoveg.Contains("basszatokmeg") || szoveg.Contains("baszás") || szoveg.Contains("baszas")
               || szoveg.Contains("köcsög") || szoveg.Contains("kocsog") || szoveg.Contains("balfasz")
               || szoveg.Contains("prosti") || szoveg.Contains("prostituált") || szoveg.Contains("prostitualt")
               || szoveg.Contains("kurwa") || szoveg.Contains("penis") || szoveg.Contains("pussy") || szoveg.Contains("dick")
               || szoveg.Contains("cock") || szoveg.Contains("sex") || szoveg.Contains("szaros")
               || szoveg.Contains("gecis") || szoveg.Contains("ribanc") || szoveg.Contains("Anyád") || szoveg.Contains("Buzi") || szoveg.Contains("Fasz") || szoveg.Contains("Geci")
               || szoveg.Contains("Kurva") || szoveg.Contains("Pöcs") || szoveg.Contains("Pénisz") || szoveg.Contains("Pina")
               || szoveg.Contains("Punci") || szoveg.Contains("Gyökér") || szoveg.Contains("Fasszopó")
               || szoveg.Contains("Fasszopo") || szoveg.Contains("Penisz") || szoveg.Contains("Pocs")
               || szoveg.Contains("Gyoker") || szoveg.Contains("Sperma") || szoveg.Contains("Szex") || szoveg.Contains("Szopás")
               || szoveg.Contains("Szopas") || szoveg.Contains("Farok") || szoveg.Contains("Cici")
               || szoveg.Contains("Idióta") || szoveg.Contains("Idiota") || szoveg.Contains("Retardált")
               || szoveg.Contains("Retardalt") || szoveg.Contains("Bazdmeg")
               || szoveg.Contains("Basszátokmeg") || szoveg.Contains("Basszatokmeg") || szoveg.Contains("Baszás") || szoveg.Contains("Baszas")
               || szoveg.Contains("Köcsög") || szoveg.Contains("Kocsog") || szoveg.Contains("Balfasz")
               || szoveg.Contains("Prosti") || szoveg.Contains("Prostituált") || szoveg.Contains("Prostitualt")
               || szoveg.Contains("Kurwa") || szoveg.Contains("Penis") || szoveg.Contains("Pussy") || szoveg.Contains("Dick")
               || szoveg.Contains("Cock") || szoveg.Contains("Sex") || szoveg.Contains("Szaros")
               || szoveg.Contains("Gecis") || szoveg.Contains("Ribanc") || szoveg.Contains("ANYÁD") || szoveg.Contains("BUZI") || szoveg.Contains("FASZ") || szoveg.Contains("GECI")
               || szoveg.Contains("KURVA") || szoveg.Contains("PÖCS") || szoveg.Contains("PÉNISZ") || szoveg.Contains("PINA")
               || szoveg.Contains("PUNCI") || szoveg.Contains("GYÖKÉR") || szoveg.Contains("FASSZOPÓ")
               || szoveg.Contains("FASSZOPO") || szoveg.Contains("PENISZ") || szoveg.Contains("POCS")
               || szoveg.Contains("GYOKER") || szoveg.Contains("SPERMA") || szoveg.Contains("SZEX") || szoveg.Contains("SZOPAS")
               || szoveg.Contains("SZOPÁS") || szoveg.Contains("FAROK") || szoveg.Contains("CICI")
               || szoveg.Contains("IDIÓTA") || szoveg.Contains("IDIOTA") || szoveg.Contains("RETARDÁLT")
               || szoveg.Contains("RETARDÁLT") || szoveg.Contains("BAZDMEG")
               || szoveg.Contains("BASSZÁTOKMEG") || szoveg.Contains("BASSZATOKMEG") || szoveg.Contains("BASZÁS") || szoveg.Contains("BASZAS")
               || szoveg.Contains("KÖCSÖG") || szoveg.Contains("KOCSOG") || szoveg.Contains("BALFASZ")
               || szoveg.Contains("PROSTI") || szoveg.Contains("PROSTITUÁLT") || szoveg.Contains("PROSTITUALT")
               || szoveg.Contains("KURWA") || szoveg.Contains("PENIS") || szoveg.Contains("PUSSY") || szoveg.Contains("DICK")
               || szoveg.Contains("COCK") || szoveg.Contains("SEX") || szoveg.Contains("SZAROS")
               || szoveg.Contains("GECIS") || szoveg.Contains("RIBANC") || szoveg.Contains("ondó")
               || szoveg.Contains("Ondó") || szoveg.Contains("getszi")
               || szoveg.Contains("GETSZI") || szoveg.Contains("Nyomorék") || szoveg.Contains("nyomorék") || szoveg.Contains("NYOMORÉK")
               || szoveg.Contains("Nyomorek") || szoveg.Contains("nyomorek") || szoveg.Contains("NYOMOREK") || szoveg.Contains("anyad")
               || szoveg.Contains("Anyad") || szoveg.Contains("ANYAD") || szoveg.Contains("nigger") || szoveg.Contains("NIGGER")
               || szoveg.Contains("Nigger") || szoveg.Contains("Cigány") || szoveg.Contains("cigany"))
                           {
                               EmbedBuilder eb = new EmbedBuilder();
                               _ = context.Message.DeleteAsync();
                               eb.AddField("Irgum-burgum!", $"Kérlek többször ne káromkodj!\n**{context.User.Username}** üzenete:\n ```{szoveg.Replace("anyád", "anya").Replace("buzi", "ferde hajlamú").Replace("fasz", "himbilimbi").Replace("geci", "hímivarsejt").Replace("kurva", "sarkcsillag").Replace("pöcs", "pömpölő").Replace("fasz", "fütyülő").Replace("pina", "lyuk").Replace("gyökér", "fafej").Replace("fasszopó", "fadarab nyalogató").Replace("pénisz", "pömpölő").Replace("szopás", "gáz").Replace("sperma", "fehér trutyi").Replace("szex", "etye-petye").Replace("farok", "dárda").Replace("cici", "didi").Replace("idióta", "tökfej").Replace("retardált", "háborodott").Replace("bazdmeg", "csináld meg").Replace("basszátok meg", "csináljátok meg").Replace("baszás", "huncutkodás").Replace("köcsög", "váza").Replace("balfasz", "ügyetlen").Replace("prosti", "munkás").Replace("prostituált", "munkás").Replace("kurwa", "lengyel munkás").Replace("penis", "angol férfi botocska").Replace("pussy", "angol női szeméremtest").Replace("dick", "angol férfi mágikus rúd").Replace("gecis", "nagyon trutyis").Replace("ribanc", "jól öltözött").Replace("punci", "szeméremtest").Replace("fasz", "botocska")}```");
                               eb.WithColor(Discord.Color.Orange);
                               avatar = context.Message.Author.GetAvatarUrl();
                               eb.WithAuthor(context.Message.Author.Username, avatar);
                               await context.Channel.SendMessageAsync("", false, eb.Build());
                               eb.WithCurrentTimestamp();
                               Console.WriteLine($"{szoveg}");
                           }*/
                #endregion
                #region KREDIT LEKERDEZES (78)
                if (parancs == ".kredit")
                {
                    EmbedBuilder eb = new EmbedBuilder();
                    try
                    {
                        user = context.Message.Author.Mention;
                        masodikresz = tomb[1];
                        replacement = masodikresz.Replace("@", "");
                        replacemente = replacement.Replace("<", "");
                        replacementk = replacemente.Replace(">", "");
                        if (masodikresz.Length != 0)
                        {
                            string path = $@"Bankok\{replacementk}bankja.txt";
                            van = File.Exists(path);
                            if (van == true)
                            {
                                foreach (var item in File.ReadAllLines($@"Bankok\{replacementk}bankja.txt"))
                                {
                                    adatok Adat = new adatok(item);
                                    lista.Add(Adat);
                                }
                                foreach (var item in lista)
                                {
                                    if (item.allapot == "nyitva")
                                    {
                                        kreditek = item.penz;
                                        eb.AddField("Kredit lekérdezés:", $"A megemlített személynek összesen **{kreditek}** :coin: kreditje van.");
                                        eb.WithColor(Discord.Color.Blue);
                                        avatar = context.Message.Author.GetAvatarUrl();
                                        eb.WithAuthor(context.Message.Author.Username, avatar);
                                        await context.Channel.SendMessageAsync("", false, eb.Build());
                                    }
                                    else
                                    {
                                        eb.AddField("Kredit lekérdezés:", $"A megemlített személy fiókja zárva van.");
                                        eb.WithColor(Discord.Color.Red);
                                        avatar = context.Message.Author.GetAvatarUrl();
                                        eb.WithAuthor(context.Message.Author.Username, avatar);
                                        await context.Channel.SendMessageAsync("", false, eb.Build());
                                    }
                                }
                            }
                            else
                            {
                                eb.AddField("Kredit lekérdezés:", $"Nem találtam ilyen személyt.");
                                eb.WithColor(Discord.Color.Red);
                                avatar = context.Message.Author.GetAvatarUrl();
                                eb.WithAuthor(context.Message.Author.Username, avatar);
                                await context.Channel.SendMessageAsync("", false, eb.Build());
                            }
                        }
                    }
                    catch
                    {
                        string path = $@"Bankok\{context.User.Id}bankja.txt";
                        van = File.Exists(path);
                        if (van == true)
                        {
                            foreach (var item in File.ReadAllLines($@"Bankok\{context.User.Id}bankja.txt"))
                            {
                                adatok Adat = new adatok(item);
                                lista.Add(Adat);
                            }
                            foreach (var item in lista)
                            {
                                kreditek = item.penz;
                                eb.AddField("Kredit lekérdezés:", $"Neked összesen **{kreditek}** :coin: kredited van.");
                                eb.WithColor(Discord.Color.Blue);
                                avatar = context.Message.Author.GetAvatarUrl();
                                eb.WithAuthor(context.Message.Author.Username, avatar);
                                await context.Channel.SendMessageAsync("", false, eb.Build());
                            }
                        }
                    }
                }
                lista.Clear();
                #endregion
                #region KREDIT KULDES (131)
                if (parancs == ".küld")
                {
                    EmbedBuilder eb = new EmbedBuilder();
                    masodikresz = tomb[1];
                    ertek = tomb[2];
                    replacement = masodikresz.Replace("@", "");
                    replacemente = replacement.Replace("<", "");
                    replacementk = replacemente.Replace(">", "");
                    if (masodikresz.Length != 0)
                    {
                        string userpath = $@"Bankok\{context.User.Id}bankja.txt";
                        van = File.Exists(userpath);
                        string path = $@"Bankok\{replacementk}bankja.txt";
                        vanilyenfelhasznalo = File.Exists(path);
                        if (van == true && vanilyenfelhasznalo == false)
                        {
                            eb.AddField("Kredit küldés:", $"Nem találtam ilyen személyt.");
                            eb.WithColor(Discord.Color.Red);
                            avatar = context.Message.Author.GetAvatarUrl();
                            eb.WithAuthor(context.Message.Author.Username, avatar);
                            await context.Channel.SendMessageAsync("", false, eb.Build());
                        }
                        if (van == false && vanilyenfelhasznalo == false)
                        {
                            eb.AddField("Kredit küldés", $"Neked még nincs fiókod.\nLétrehozhatsz egyet a **.bank létrehoz** paranccsal.");
                            eb.WithColor(Discord.Color.Red);
                            avatar = context.Message.Author.GetAvatarUrl();
                            eb.WithAuthor(context.Message.Author.Username, avatar);
                            await context.Channel.SendMessageAsync("", false, eb.Build());
                        }
                        if (van == false && vanilyenfelhasznalo == true)
                        {
                            eb.AddField("Kredit küldés", $"Neked még nincs fiókod.\nLétrehozhatsz egyet a **.bank létrehoz** paranccsal.");
                            eb.WithColor(Discord.Color.Red);
                            avatar = context.Message.Author.GetAvatarUrl();
                            eb.WithAuthor(context.Message.Author.Username, avatar);
                            await context.Channel.SendMessageAsync("", false, eb.Build());
                        }
                        if (van == true && vanilyenfelhasznalo == true)
                        {
                            foreach (var item in File.ReadAllLines($@"Bankok\{context.User.Id}bankja.txt"))
                            {
                                adatok Adat = new adatok(item);
                                lista.Add(Adat);
                            }
                            foreach (var item in lista)
                            {
                                if (item.allapot == "zárva")
                                {
                                    sajatzarva = true;
                                }
                                else
                                {
                                    sajatzarva = false;
                                    sajatpenz = item.penz;
                                }
                            }
                            foreach (var itemk in File.ReadAllLines($@"Bankok\{replacementk}bankja.txt"))
                            {
                                adatok Adat = new adatok(itemk);
                                lista.Add(Adat);
                            }
                            foreach (var itemk in lista)
                            {
                                if (itemk.allapot == "zárva")
                                {
                                    emlitettzarva = true;
                                }
                                else
                                {
                                    emlitettzarva = false;
                                    emlitettpenze = itemk.penz;
                                }
                            }
                            if (sajatzarva == false && emlitettzarva == true)
                            {
                                eb.AddField("Kredit küldés", $"A megemlített személy fiókja zárva van.");
                                eb.WithColor(Discord.Color.Red);
                                avatar = context.Message.Author.GetAvatarUrl();
                                eb.WithAuthor(context.Message.Author.Username, avatar);
                                await context.Channel.SendMessageAsync("", false, eb.Build());
                            }
                            if (sajatzarva == true && emlitettzarva == false)
                            {
                                eb.AddField("Kredit küldés", $"A fiókod zárva van. Kinyithatod a **.bank kinyit** paranccsal.");
                                eb.WithColor(Discord.Color.Red);
                                avatar = context.Message.Author.GetAvatarUrl();
                                eb.WithAuthor(context.Message.Author.Username, avatar);
                                await context.Channel.SendMessageAsync("", false, eb.Build());
                            }
                            if (sajatzarva == true && emlitettzarva == true)
                            {
                                eb.AddField("Kredit küldés", $"A fiókod zárva van. Kinyithatod a **.bank kinyit** paranccsal.");
                                eb.WithColor(Discord.Color.Red);
                                avatar = context.Message.Author.GetAvatarUrl();
                                eb.WithAuthor(context.Message.Author.Username, avatar);
                                await context.Channel.SendMessageAsync("", false, eb.Build());
                            }
                            if (sajatzarva == false && emlitettzarva == false)
                            {
                                if (int.Parse(ertek) > sajatpenz)
                                {
                                    eb.AddField("Kredit küldés", $"Nincs elég kredited!");
                                    eb.WithColor(Discord.Color.Red);
                                    avatar = context.Message.Author.GetAvatarUrl();
                                    eb.WithAuthor(context.Message.Author.Username, avatar);
                                    await context.Channel.SendMessageAsync("", false, eb.Build());
                                }
                                else
                                {
                                    int sajatpenzlesz = sajatpenz - int.Parse(ertek);
                                    int emlitettpenzlesz = emlitettpenze + int.Parse(ertek);
                                    string text = File.ReadAllText($@"Bankok\{context.User.Id}bankja.txt");
                                    text = text.Replace($"{sajatpenz}", $"{sajatpenzlesz}");
                                    File.WriteAllText($@"Bankok\{context.User.Id}bankja.txt", text);
                                    string textk = File.ReadAllText($@"Bankok\{replacementk}bankja.txt");
                                    textk = textk.Replace($"{emlitettpenze}", $"{emlitettpenzlesz}");
                                    File.WriteAllText($@"Bankok\{replacementk}bankja.txt", textk);
                                    eb.AddField("Kredit küldés", $"Sikeresen küldtél **{ertek}** kreditet a megemlített személynek.");
                                    eb.WithColor(Discord.Color.Green);
                                    avatar = context.Message.Author.GetAvatarUrl();
                                    eb.WithAuthor(context.Message.Author.Username, avatar);
                                    await context.Channel.SendMessageAsync("", false, eb.Build());
                                }
                            }
                        }
                    }
                    lista.Clear();
                }
                    #endregion
                #region JELSZO RABLASHOZ
                    if (parancs == ".jelszó")
                    {
                        string path = ($@"Rablas\{context.User.Id}.txt");
                        van = File.Exists(path);
                        masodikresz = tomb[1];
                        if (van == true)
                        {
                            foreach (var sajatpenzread in File.ReadAllLines($@"Bankok\{context.User.Id}bankja.txt"))
                            {
                                adatok Adat = new adatok(sajatpenzread);
                                lista.Add(Adat);
                            }
                            foreach (var item in lista)
                            {
                                sajatpenz = item.penz;
                            }
                            foreach (var itemr in File.ReadAllLines($@"Rablas\{context.User.Id}.txt"))
                            {
                                rablasok Adat = new rablasok(itemr);
                                rablaslista.Add(Adat);
                            }
                            foreach (var itemr in rablaslista)
                            {
                                if (itemr.jelszo == $"{masodikresz}")
                                {
                                    replacementk = itemr.kitolrabol;
                                    foreach (var itemx in File.ReadAllLines($@"Bankok\{replacementk}bankja.txt"))
                                    {
                                        adatok Adat = new adatok(itemx);
                                        lista.Add(Adat);
                                    }
                                    foreach (var itemx in lista)
                                    {
                                        emlitettpenze = itemx.penz;
                                    }
                                    EmbedBuilder eb = new EmbedBuilder();
                                    Random randompenz = new Random();
                                    int rabolt = randompenz.Next(20, 101);
                                    eb.AddField("Kredit rablás: ", $"Sikeresen raboltál **{rabolt}** :coin: kreditet!");
                                    avatar = context.Message.Author.GetAvatarUrl();
                                    eb.WithAuthor(context.Message.Author.Username, avatar);
                                    eb.WithColor(Discord.Color.Green);
                                    await context.Channel.SendMessageAsync("", false, eb.Build());
                                    int sajatpenzlesz = sajatpenz + rabolt;
                                    int emlitettpenzlesz = emlitettpenze - rabolt;
                                    string text = File.ReadAllText($@"Bankok\{context.User.Id}bankja.txt");
                                    text = text.Replace($"{sajatpenz}", $"{sajatpenzlesz}");
                                    File.WriteAllText($@"Bankok\{context.User.Id}bankja.txt", text);
                                    string textk = File.ReadAllText($@"Bankok\{replacementk}bankja.txt");
                                    textk = textk.Replace($"{emlitettpenze}", $"{emlitettpenzlesz}");
                                    File.WriteAllText($@"Bankok\{replacementk}bankja.txt", textk);
                                    Console.WriteLine($"{emlitettpenze}, {emlitettpenzlesz}, {replacementk}");
                                    File.Delete($@"Rablas\{context.User.Id}.txt");
                                    break;
                                }
                                else
                                {
                                    EmbedBuilder eb = new EmbedBuilder();
                                    eb.AddField("Kredit rablás:", $"Helytelen jelszót adtál meg, ezért a rablás félbeszakadt!");
                                    eb.WithColor(Discord.Color.Red);
                                    avatar = context.Message.Author.GetAvatarUrl();
                                    eb.WithAuthor(context.Message.Author.Username, avatar);
                                    await context.Channel.SendMessageAsync("", false, eb.Build());
                                    File.Delete($@"Rablas\{context.User.Id}.txt");
                                    break;
                                }
                            }
                        }
                    }
                    lista.Clear();
                    rablaslista.Clear();
                    #endregion
                #region KREDIT RABLAS
                    if (parancs == ".rabol")
                    {
                    EmbedBuilder eb = new EmbedBuilder();
                    try
                        {
                        user = context.Message.Author.Mention;
                        masodikresz = tomb[1];
                        replacement = masodikresz.Replace("@", "");
                        replacemente = replacement.Replace("<", "");
                        replacementk = replacemente.Replace(">", "");
                        string usercheck = masodikresz.Replace("@", "@!");
                        string utvonal = $@"Rablas\{context.User.Id}.txt";
                        bool eppenrabol = File.Exists(utvonal);
                            if (eppenrabol == true)
                            {
                               
                            }
                            else
                            {
                                eppenrabol = false;
                            }
                        if (user == usercheck)
                        {
                            eb.AddField("Kredit rablás", $"Saját magadtól nem rabolhatsz kreditet!");
                            eb.WithColor(Discord.Color.Red);
                            avatar = context.Message.Author.GetAvatarUrl();
                            eb.WithAuthor(context.Message.Author.Username, avatar);
                            await context.Channel.SendMessageAsync("", false, eb.Build());
                        }
                        else
                        {
                            if (masodikresz.Length != 0)
                            {
                                string path = $@"Bankok\{replacementk}bankja.txt";
                                van = File.Exists(path);
                                if (van == true)
                                {
                                    foreach (var item in File.ReadAllLines($@"Bankok\{replacementk}bankja.txt"))
                                    {
                                        adatok Adat = new adatok(item);
                                        lista.Add(Adat);
                                    }
                                    foreach (var itemx in File.ReadAllLines($@"Bankok\{context.User.Id}bankja.txt"))
                                    {
                                        adatok Adat = new adatok(itemx);
                                        lista.Add(Adat);
                                    }
                                    foreach (var itemx in lista)
                                    {
                                        if (itemx.allapot == "zárva")
                                        {
                                            sajatzarva = true;
                                        }
                                        else
                                        {
                                            sajatzarva = false;
                                            sajatpenz = itemx.penz;
                                        }
                                    }
                                    foreach (var item in lista)
                                    {
                                        if (item.allapot == "nyitva" && item.penz >= 100 && sajatzarva == false && eppenrabol == false)
                                        {
                                            emlitettpenze = item.penz;
                                            Random random = new Random();
                                            var szavak = new List<string>()
                                            {
                                              "udvarias","csomag","kerékpár","bicikli","tornaterem","kosárlabda","templom","focilabda","ceruza","stadion","tavasz","fekete","fehér","fénykép",
                                                "internet","fizetés","szemöldök","szempilla","hitelkártya","monitor","katica","színpad","dinnye","tűzoltó","szivacs","macska","kutya"
                                            };
                                            string ajelszo = szavak[random.Next(0, szavak.Count)];
                                            string osszekevert = new string(ajelszo.ToCharArray().OrderBy(s => (random.Next(2) % 2) == 0).ToArray());
                                            EmbedBuilder ebb = new EmbedBuilder();
                                            ebb.AddField($"A tűzfal aktív!", $"Úgy tűnik, hogy a banknak a tűzfala aktív, és szükség van egy jelszóra, amivel kitudom kapcsolni.\nDe sebaj! Megszereztem a jelszót, csak sajnos a betűk összekeveredtek.\nA jelszó a következő:```{osszekevert}```\nMegtudod fejteni? :thinking: \nA jelszó beírásához használd a **.jelszó <jelszó>** parancsot!");
                                            ebb.WithColor(Discord.Color.DarkOrange);
                                            avatar = context.Message.Author.GetAvatarUrl();
                                            ebb.WithAuthor(context.Message.Author.Username, avatar);
                                            await context.Channel.SendMessageAsync("", false, ebb.Build());
                                            kreditek = item.penz;
                                            File.WriteAllText($@"Rablas\{context.User.Id}.txt",$"{replacementk};{ajelszo};0");
                                              /*  for (int i = 0; i < 30; i++)
                                                {
                                                    Thread.Sleep(1000);
                                                    if (i == 29)
                                                    {
                                                        File.Delete($@"Rablas\{context.User.Id}.txt");
                                                        EmbedBuilder ebbk = new EmbedBuilder();
                                                        ebbk.AddField($"Lejárt az időd!", $"A bank visszautasította a csatlakozási kérésed, így sajnos nem sikerült kreditet rabolnod 😦");
                                                        ebbk.WithColor(Discord.Color.DarkOrange);
                                                        avatar = context.Message.Author.GetAvatarUrl();
                                                        ebbk.WithAuthor(context.Message.Author.Username, avatar);
                                                        await context.Channel.SendMessageAsync("", false, ebbk.Build());
                                                    }
                                            }
*/                                            break;
                                        }
                                        else if(eppenrabol == true)
                                        {
                                            eb.AddField("Kredit rablás:", $"Hiba! Még továbbra is rabolsz!");
                                            eb.WithColor(Discord.Color.Red);
                                            avatar = context.Message.Author.GetAvatarUrl();
                                            eb.WithAuthor(context.Message.Author.Username, avatar);
                                            await context.Channel.SendMessageAsync("", false, eb.Build());
                                            break;
                                        }
                                            else
                                            {
                                                eb.AddField("Kredit rablás", $"A megemlített személy fiókja zárva van, vagy nincs elég kreditje.");
                                                eb.WithColor(Discord.Color.Red);
                                                avatar = context.Message.Author.GetAvatarUrl();
                                                eb.WithAuthor(context.Message.Author.Username, avatar);
                                                await context.Channel.SendMessageAsync("", false, eb.Build());
                                                break;
                                            }
                                    }
                                }
                                else
                                {
                                    eb.AddField("Kredit rablás", $"Nem találtam ilyen személyt.");
                                    eb.WithColor(Discord.Color.Red);
                                    avatar = context.Message.Author.GetAvatarUrl();
                                    eb.WithAuthor(context.Message.Author.Username, avatar);
                                    await context.Channel.SendMessageAsync("", false, eb.Build());
                                }
                            }
                        }
                    }
                    catch
                    {
                        
                    }
                }
                    lista.Clear();
                    #endregion
                #region TITKOSITAS (161)
                string currenthash = "";
                if (parancs == ".hash")
                {
                    EmbedBuilder eb = new EmbedBuilder();
                    try
                    {
                        user = context.Message.Author.Mention;
                        masodikresz = tomb[1];
                        if (masodikresz == "gen")
                        {
                            var betukszamokkarakterek = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789§'+!%/=()?:_~^|Ä€÷×äđĐ[]łŁ$ß¤<>#&@{}<*";
                            var stringbszk = new char[12];
                            var random = new Random();
                            for (int i = 0; i < stringbszk.Length; i++)
                            {
                                stringbszk[i] = betukszamokkarakterek[random.Next(betukszamokkarakterek.Length)];
                            }
                            _ = context.Message.DeleteAsync();
                            var generated = new String(stringbszk);
                            eb.WithColor(Discord.Color.Green);
                            avatar = context.Message.Author.GetAvatarUrl();
                            eb.WithAuthor(context.Message.Author.Username, avatar);
                            eb.AddField("Sikrerült!", $"Az új generált kulcsod: **{generated}**\n A beállításhoz használd a: ```.hash {generated}``` parancsot!", false);
                            await context.User.SendMessageAsync("", false, eb.Build());
                        }
                        else
                        {
                            string path = $@"Titkositasok\{context.User.Id}titkositas.txt";
                            van = File.Exists(path);
                            if (van == true)
                            {
                                _ = context.Message.DeleteAsync();
                                File.WriteAllText($@"Titkositasok\{context.User.Id}titkositas.txt", $"{masodikresz}");
                                eb.AddField("Sikrerült!", $"Az új kulcsod: **{masodikresz}**", false);
                                eb.WithColor(Discord.Color.Green);
                                avatar = context.Message.Author.GetAvatarUrl();
                                eb.WithAuthor(context.Message.Author.Username, avatar);
                                await context.User.SendMessageAsync("", false, eb.Build());
                            }
                            else
                            {
                                _ = context.Message.DeleteAsync();
                                File.WriteAllText($@"Titkositasok\{context.User.Id}titkositas.txt", $"{masodikresz}");
                                eb.AddField("Sikrerült!", $"Az új kulcsod: **{masodikresz}**", false);
                                avatar = context.Message.Author.GetAvatarUrl();
                                eb.WithAuthor(context.Message.Author.Username, avatar);
                                eb.WithColor(Discord.Color.Green);
                                await context.User.SendMessageAsync("", false, eb.Build());
                            }

                        }
                    }
                    catch
                    {

                    }
                    hashlista.Clear();
                }
                string decrypted = "";
                string encrypted = "";
                if (parancs == ".ts")
                {
                    EmbedBuilder eb = new EmbedBuilder();
                    masodikresz = tomb[1];
                    foreach (var item in File.ReadAllLines($@"Titkositasok\{context.User.Id}titkositas.txt"))
                    {
                        titkositasok Titkositas = new titkositasok(item);
                        hashlista.Add(Titkositas);
                    }
                    foreach (var item in hashlista)
                    {
                        currenthash = item.hash;
                    }
                    string path = $@"Titkositasok\{context.User.Id}titkositas.txt";
                    van = File.Exists(path);
                    string text = "";
                    text = szoveg.Replace(".ts", "");
                    if (van == true)
                    {
                        _ = context.Message.DeleteAsync();
                        string encryptedtext = szoveg;
                        try
                        {
                            byte[] data = UTF8Encoding.UTF8.GetBytes(text);
                            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                            {
                                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(currenthash));
                                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                                {
                                    ICryptoTransform transform = tripDes.CreateEncryptor();
                                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                                    encrypted = Convert.ToBase64String(results, 0, results.Length);
                                }
                            }
                            eb.AddField("Üzenete:", $"```{encrypted}```", false);
                            avatar = context.Message.Author.GetAvatarUrl();
                            eb.WithAuthor(context.Message.Author.Username, avatar);
                            eb.WithColor(Discord.Color.Blue);
                            eb.WithCurrentTimestamp();
                            await context.Channel.SendMessageAsync("", false, eb.Build());
                        }
                        catch
                        {

                        }
                    }
                    hashlista.Clear();
                }
                if (parancs == ".vts")
                {
                    EmbedBuilder eb = new EmbedBuilder();
                    try
                    {
                        masodikresz = tomb[1];
                        user = context.Message.Author.Mention;
                        foreach (var item in File.ReadAllLines($@"Titkositasok\{context.User.Id}titkositas.txt"))
                        {
                            titkositasok Titkositas = new titkositasok(item);
                            hashlista.Add(Titkositas);
                        }
                        foreach (var item in hashlista)
                        {
                            currenthash = item.hash;
                        }
                        string path = $@"Titkositasok\{context.User.Id}titkositas.txt";
                        van = File.Exists(path);
                        if (van == true)
                        {
                            byte[] data = Convert.FromBase64String(masodikresz);
                            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                            {
                                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(currenthash));
                                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                                {
                                    ICryptoTransform transform = tripDes.CreateDecryptor();
                                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                                    decrypted = UTF8Encoding.UTF8.GetString(results);
                                }
                            }
                            _ = context.Message.DeleteAsync();
                            eb.AddField("A dekódolt üzenet:", $"```{decrypted}```", false);
                            avatar = context.Message.Author.GetAvatarUrl();
                            eb.WithAuthor(context.Message.Author.Username, avatar);
                            eb.WithColor(Discord.Color.Blue);
                            eb.WithCurrentTimestamp();
                            await context.User.SendMessageAsync("", false, eb.Build());
                        }
                    }
                    catch
                    {
                        _ = context.Message.DeleteAsync();
                        eb.AddField("Hiba!", "Az üzenet máshogy lett titkosítva, így nemtudom dekódolni, mert neked nem az a kulcs van megadva, mint amivel titkosítva lett!");
                        avatar = context.Message.Author.GetAvatarUrl();
                        eb.WithAuthor(context.Message.Author.Username, avatar);
                        eb.WithColor(Discord.Color.Red);
                        await context.Channel.SendMessageAsync("", false, eb.Build());
                    }
                }
                hashlista.Clear();
                lista.Clear();
                #endregion
                #region IDOJARAS LEKERDEZES (50)
                if (parancs == ".idő")
                {
                    try
                    {
                        masodikresz = tomb[1];
                    }
                    catch
                    {
                        masodikresz = "Budapest";
                    }
                    EmbedBuilder eb = new EmbedBuilder();
                    string url = $"https://api.openweathermap.org/data/2.5/weather?q={masodikresz}&lang=hu&appid=7e74860577b6d3d41092af1e64383b03&units=metric";
                    var json = new WebClient().DownloadString($"{url}");
                    idojarasadatok.root Info = JsonConvert.DeserializeObject<idojarasadatok.root>(json);
                    string icon = $"https://openweathermap.org/img/w/" + Info.weather[0].icon + ".png";
                    string main = Info.weather[0].main;
                    string leiras = Info.weather[0].description;
                    string napfelkelte = convertDateTime(Info.sys.sunrise).ToString("HH:mm");
                    string napnyugta = convertDateTime(Info.sys.sunset).ToString("HH:mm");
                    string szel = Info.wind.speed.ToString();
                    string homerseklet = Info.main.temp.ToString();
                    string erzekelt = Info.main.feels_like.ToString();
                    double homersekletkerekit = Math.Round(Convert.ToDouble(homerseklet), 1);
                    double erzekelthomersekletkerekit = Math.Round(Convert.ToDouble(erzekelt), 1);
                    string country = Info.sys.country.ToLower();
                    byte[] bytes = Encoding.Default.GetBytes(main);
                    main = Encoding.UTF8.GetString(bytes);
                    bytes = Encoding.Default.GetBytes(leiras);
                    leiras = Encoding.UTF8.GetString(bytes);
                    eb.WithTitle($":flag_{country}: {masodikresz} időjárása jelenleg:");
                    eb.AddField($"Állapot:", $"{leiras}");
                    eb.AddField("Hőmérséklet:", $"{homersekletkerekit}°C", true);
                    eb.AddField("Hőérzet:", $"{erzekelthomersekletkerekit}°C", true);
                    eb.AddField("Szél:", $"{szel}m/s", false);
                    eb.AddField("Napkelte:", $"{napfelkelte}", true);
                    eb.AddField("Napnyugta:", $"{napnyugta}", true);
                    eb.WithFooter(footer => footer.Text = "Forrás: OpenWeatherMap");
                    eb.WithThumbnailUrl(icon);
                    eb.WithColor(Discord.Color.Blue);
                    eb.WithCurrentTimestamp();
                    avatar = context.Message.Author.GetAvatarUrl();
                    eb.WithAuthor(context.Message.Author.Username, avatar);
                    await context.Channel.SendMessageAsync("", false, eb.Build());
                }
                DateTime convertDateTime(long sc)
                {
                    DateTime nap = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).ToLocalTime();
                    nap = nap.AddSeconds(sc).ToLocalTime();
                    return nap;
                }
                #endregion
                #region COVID LEKERDEZES (38)
                if (parancs == ".covid")
                {
                    try
                    {
                        masodikresz = tomb[1];
                    }
                    catch
                    {
                        masodikresz = "Hungary";
                    }
                    var client = new RestClient($"https://covid-19-coronavirus-statistics.p.rapidapi.com/v1/total?country={masodikresz}");
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("X-RapidAPI-Key", "2243bfcfdbmsh2b3f4f7d3ee8e5dp173223jsn3de571e9edcf");
                    request.AddHeader("X-RapidAPI-Host", "covid-19-coronavirus-statistics.p.rapidapi.com");
                    IRestResponse response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        var content = response.Content;
                        covid.root Info = JsonConvert.DeserializeObject<covid.root>(content);
                        string halalozasok = Info.data.deaths.ToString();
                        string összes = Info.data.confirmed.ToString();
                        eb.WithTitle($"{masodikresz} COVID adatai:");
                        eb.AddField("Összes eset:", $"{összes}", true);
                        eb.AddField("Elhunytak:", $"{halalozasok}", true);
                        eb.WithColor(Discord.Color.Blue);
                        eb.WithThumbnailUrl("https://media4.giphy.com/media/zBnntUUZUtnrTwAP2H/giphy.gif?cid=790b7611046ed271e401ae00841a7c55b8ec9fb3a077f76a&rid=giphy.gif&ct=g");
                        eb.WithFooter(footer => footer.Text = "Forrás: WHO");
                        eb.WithCurrentTimestamp();
                        avatar = context.Message.Author.GetAvatarUrl();
                        eb.WithAuthor(context.Message.Author.Username, avatar);
                        await context.Channel.SendMessageAsync("", false, eb.Build());
                    }
                    else
                    {
                        await context.Channel.SendMessageAsync("ajaj");
                    }
                }
                #endregion
                #region URL ROVIDITES (29)
                if (parancs == ".url")
                {
                    masodikresz = tomb[1];
                    if (masodikresz.StartsWith("https://") || masodikresz.StartsWith("https://"))
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        _ = context.Message.DeleteAsync();
                        var client = new RestClient($"https://url-shortener20.p.rapidapi.com/shorten?url={masodikresz}");
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("content-type", "application/json");
                        request.AddHeader("X-RapidAPI-Key", "2243bfcfdbmsh2b3f4f7d3ee8e5dp173223jsn3de571e9edcf");
                        request.AddHeader("X-RapidAPI-Host", "url-shortener20.p.rapidapi.com");
                        request.AddParameter("application/json", "{\r\"url\": \"https://www.bbc.com/sport/football\"\r", ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        var content = response.Content;
                        url Info = JsonConvert.DeserializeObject<url>(content);
                        string url = Info.short_link;
                        eb.AddField("Az eredeti url:", $"{masodikresz}");
                        eb.AddField("Rövidített url:", $"{url}");
                        eb.WithColor(Discord.Color.Blue);
                        eb.WithCurrentTimestamp();
                        avatar = context.Message.Author.GetAvatarUrl();
                        eb.WithAuthor(context.Message.Author.Username, avatar);
                        eb.WithThumbnailUrl("https://static.thenounproject.com/png/3127673-200.png");
                        await context.Channel.SendMessageAsync("", false, eb.Build());
                    }
                }
                #endregion
                #region FORDITAS (28)
                if (parancs == ".fordít")
                {
                    EmbedBuilder eb = new EmbedBuilder();
                    masodikresz = tomb[1];
                    replacement = szoveg.Replace(".fordít", "");
                    replacemente = replacement.Replace($"{masodikresz} ", "");
                    var client = new RestClient("https://translator82.p.rapidapi.com/api/translate");
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("X-RapidAPI-Key", "2243bfcfdbmsh2b3f4f7d3ee8e5dp173223jsn3de571e9edcf");
                    request.AddHeader("X-RapidAPI-Host", "translator82.p.rapidapi.com");
                    request.AddParameter("application/json", $"{{\r\"language\": \"{masodikresz}\",\r\"text\": \"{replacemente}\"\r}}", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    var content = response.Content;
                    forditasok Info = JsonConvert.DeserializeObject<forditasok>(content);
                    string forditva = Info.result;
                    string errolanyelvrol = Info.detectedSourceLanguage;
                    eb.AddField($"Eredeti szöveg {errolanyelvrol}:", $"{replacemente}");
                    eb.AddField($"Fordítva {masodikresz} nyelvre:", $"{forditva}");
                    eb.WithColor(Discord.Color.Blue);
                    eb.WithCurrentTimestamp();
                    avatar = context.Message.Author.GetAvatarUrl();
                    eb.WithAuthor(context.Message.Author.Username, avatar);
                    await context.Channel.SendMessageAsync("", false, eb.Build());
                }
                #endregion
                #region SZOVEGET HANGGA (21)
                if (parancs == ".hang")
                {
                    replacement = szoveg.Replace("/hang", "");
                    var client = new RestClient("https://cloudlabs-text-to-speech.p.rapidapi.com/synthesize");
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("content-type", "application/x-www-form-urlencoded");
                    request.AddHeader("X-RapidAPI-Key", "2243bfcfdbmsh2b3f4f7d3ee8e5dp173223jsn3de571e9edcf");
                    request.AddHeader("X-RapidAPI-Host", "cloudlabs-text-to-speech.p.rapidapi.com");
                    request.AddParameter("application/x-www-form-urlencoded", $"voice_code=hu-HU-2%09&text={replacement}&speed=1.00&pitch=1.00&output_type=audio_url", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    var content = response.Content;
                    tts.root Info = JsonConvert.DeserializeObject<tts.root>(content);
                    string url = Info.result.audio_url;
                    using (var clienttoltes = new WebClient())
                    {
                        clienttoltes.DownloadFile($"{url}", $@"Hangok\{context.User.Id}.wav");
                    }
                    await context.Channel.SendFileAsync($@"Hangok\{context.User.Id}.wav");
                }
                #endregion
                #region WIKIPEDIA (80)
                if (parancs == ".wiki")
                {
                    replacement = szoveg.Replace(".wiki", "");
                    if (replacement.Length == 0)
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.AddField("Bug (informatika)", "A bug a számítógépes programhiba elterjedt elnevezése.Előfordulásakor a számítógépes szoftver" +
                            " hibás eredményt ad, vagy a tervezettől eltérően viselkedik.A legtöbb bug a programozók által a forráskódban vagy a " +
                            "programstruktúrában vétett hibák eredménye, kisebbik részüket pedig a fordítóprogram által generált hibás kód okozza.Az olyan " +
                            "programot, mely sok bugot tartalmaz, és / vagy a bugok jelentősen akadályozzák a program használatát, gyakran bugosnak nevezik.");
                        eb.WithColor(Discord.Color.Red);
                        eb.AddField("Forrás:", $"🔗 [Wikipédia](https://hu.wikipedia.org/wiki/Bug_(informatika)");
                        eb.WithFooter(footer => footer.Text = "Helytelenül használtad a parancsot! Kérlek azt is add meg, amire keresni szeretnél!");
                        eb.WithThumbnailUrl("http://www.quickmeme.com/img/7c/7cace54ced07a4649c460b72ec11c7f6537fb5276a5fdd75628e27699fc73a8f.jpg");
                        await context.Channel.SendMessageAsync("", false, eb.Build());
                    }
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    WebClient client = new WebClient();
                    string kepurlsend = string.Empty;
                    string adat = string.Empty;
                    string url = $"https://hu.wikipedia.org/w/api.php?action=parse&page={replacement}&format=json";
                    string kepurl = $"https://hu.wikipedia.org/w/api.php?action=query&titles={replacement}&prop=pageimages&format=json&pithumbsize=480";
                    //","width":100,"height":84,"pageimage":"EU-Hungary.svg
                    //,"width":100,"height":84},"pageimage":"EU-Hungary.svg"}}}}
                    var kepjson = new WebClient().DownloadString($"{kepurl}");
                    File.WriteAllText("wikikep.json", $"{kepjson}");
                    var json = new WebClient().DownloadString($"{url}");
                    wikipedia.rootx Info = JsonConvert.DeserializeObject<wikipedia.rootx>(json);
                    string cim = Info.Parse.title.ToString();
                    using (Stream stream = client.OpenRead($"http://hu.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&explaintext=1&titles={replacement}"))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        JsonSerializer ser = new JsonSerializer();
                        Result result = ser.Deserialize<Result>(new JsonTextReader(reader));

                        foreach (Page page in result.query.pages.Values)
                            adat = page.extract.ToString();
                    }
                    File.WriteAllText("wiki.txt", $"{adat}");
                    foreach (var item in File.ReadAllLines("wiki.txt"))
                    {
                        adat = item;
                        if (!item.Contains("="))
                        {
                            break;
                        }
                    }
                    char korom = '"';
                    kepurlsend = File.ReadAllText("wikikep.json");
                    replacemente = cim.Replace(" ", "_");
                    try
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.AddField($"{replacement}", $"{adat}");
                        eb.WithColor(Discord.Color.Blue);
                        eb.AddField("Forrás:", $"🔗 [Wikipédia](https://hu.wikipedia.org/wiki/{replacemente})");
                        kepurlsend = kepurlsend.Replace("}", "");
                        MatchCollection ms = Regex.Matches(kepurlsend, @"(www.+|http.+)([\s]|$)");
                        string testMatch = ms[0].Value.ToString();
                        eb.WithThumbnailUrl($"{testMatch.Replace($"{korom}", "")}");
                        stopwatch.Stop();
                        long elapsed_time = stopwatch.ElapsedMilliseconds;
                        eb.WithFooter(footer => footer.Text = $"Válaszidő: {elapsed_time}ms");
                        await context.Channel.SendMessageAsync("", false, eb.Build());
                    }
                    catch (Exception e)
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.AddField($"Hiba!", $"Nem találtam ilyen oldalt.");
                        eb.WithColor(Discord.Color.Red);
                        eb.AddField("Javaslatok:", $"🔗 [Wikipédia](https://hu.wikipedia.org/wiki/{replacemente})");
                        eb.WithFooter(footer => footer.Text = $"A kereső még nem tökéletes, ezért létező lapokra is hibát adthat ki!\nDEBUG: {url}\nKeresés: {replacement}\nHIBA:\n{e}");
                        await context.Channel.SendMessageAsync("", false, eb.Build());
                    }
                    //string kepurljo = kepurlsend.Replace("}", "");

                }
                #endregion
                #region WIKIPEDIA CIKKEK KERESESE (47)
                if (parancs == ".swiki")
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    replacement = szoveg.Replace(".swiki", "");
                    WebClient wiki = new WebClient();
                    var letoltes = wiki.DownloadString($"https://hu.wikipedia.org/w/api.php?action=opensearch&search={replacement}");
                    var jsonconvert = Newtonsoft.Json.JsonConvert.DeserializeObject(letoltes);
                    string[] eredmeny = jsonconvert.ToString().Split('[');
                    var i = eredmeny[2];
                    char q = ('"');
                    string rendbehoz = i.Replace($"{q}", "");
                    string rendbehoz2 = rendbehoz.Replace(",", "");
                    string rendbehoz3 = rendbehoz2.Replace("]", "");
                    File.WriteAllText("wiki.txt", $"{rendbehoz3}");
                    List<string> adatok = File.ReadAllLines("wiki.txt").Skip(1).ToList();
                    EmbedBuilder eb = new EmbedBuilder();
                    //var adatoks = String.Join(", ", adatok.ToArray());
                    for (int x = 0; x < adatok.Count; x++)
                    {
                        try
                        {
                            string rendbehozurl = adatok[x].Replace(" ", "%20");
                            //string url = rendbehozurl.Replace("%20", "");
                            eb.AddField($"{adatok[x]}", $"🔗 [{adatok[x]}](https://hu.wikipedia.org/wiki/%20{rendbehozurl})");
                        }
                        catch
                        {

                        }
                        //await context.Channel.SendMessageAsync($"{adatok[x]}");
                    }
                    //await context.Channel.SendMessageAsync($"{adatoks}");
                    stopwatch.Stop();
                    double elapsed_time = stopwatch.ElapsedMilliseconds;
                    eb.WithFooter(footer => footer.Text = $"Válaszidő: {elapsed_time}ms");
                    eb.WithColor(Discord.Color.Blue);
                    eb.WithThumbnailUrl("https://i.insider.com/5fbd515550e71a001155724f?width=700");
                    eb.WithTitle("Ezeket találtam:");
                    avatar = context.User.GetAvatarUrl();
                    eb.WithAuthor(context.User.Username, avatar);
                    await context.Channel.SendMessageAsync("", false, eb.Build());


                }
                #endregion
                #region EGYEB PARANCSOK
                if (parancs == "<@958043426237059132>")
                {
                    Emoji integetes = new Emoji("👋");
                    await context.Channel.SendMessageAsync(":wave:");
                    _ = context.Message.AddReactionAsync(integetes);
                }
                if (parancs == ".bin")
                {
                    masodikresz = tomb[1];
                    int szam = int.Parse(masodikresz);
                    string binary = Convert.ToString(szam, 2);
                    await context.Channel.SendMessageAsync($"{masodikresz} a 2-es számrendszerben: \n{binary}");
                }
                if (parancs == ".dec")
                {
                    masodikresz = tomb[1];
                    string dec = Convert.ToInt32(masodikresz, 2).ToString();
                    await context.Channel.SendMessageAsync($"{masodikresz} a 10-es számrendszerben: \n{dec}");
                }
                #endregion
                if (parancs == ".megölel")
                {
                    string url = "";
                    try
                    {
                        masodikresz = tomb[1];
                        if (masodikresz.StartsWith("<@"))
                        {
                            Random random = new Random();
                            int randomgif = random.Next(1, 11);
                            replacement = masodikresz.Replace("<@", "<@!");
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.AddField("Megölelve! :hugging: ", $"{context.Message.Author.Mention} megölelte {masodikresz}-t!");
                            if (randomgif == 1)
                            {
                                url = ("https://stream.data.hu/get/13387020/olel1.gif");
                            }
                            if (randomgif == 2)
                            {
                                url = ("https://stream.data.hu/get/13387021/olel2.gif");
                            }
                            if (randomgif == 3)
                            {
                                url = ("https://stream.data.hu/get/13387018/olel3.gif");
                            }
                            if (randomgif == 4)
                            {
                                url = ("https://stream.data.hu/get/13387019/olel4.gif");
                            }
                            if (randomgif == 5)
                            {
                                url = ("https://stream.data.hu/get/13387022/olel5.gif");
                            }
                            if (randomgif == 6)
                            {
                                url = ("https://stream.data.hu/get/13387000/olel6.gif");
                            }
                            if (randomgif == 7)
                            {
                                url = ("https://stream.data.hu/get/13387023/olel7.gif");
                            }
                            if (randomgif == 8)
                            {
                                url = ("https://stream.data.hu/get/13387025/olel8.gif");
                            }
                            if (randomgif == 9)
                            {
                                url = ("https://stream.data.hu/get/13387024/olel9.gif");
                            }
                            if (randomgif == 10)
                            {
                                url = ("https://stream.data.hu/get/13387027/olel10.gif");
                            }
                            eb.WithColor(Discord.Color.DarkGreen);
                            eb.WithImageUrl(url);
                            await context.Channel.SendMessageAsync("", false, eb.Build());
                        }
                    }
                    catch
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        url = "https://stream.data.hu/get/13387017/olel0.gif";
                        eb.AddField("Megölelve! :hugging: ", $"<@958043426237059132> megölelte {context.Message.Author.Mention}-t!");
                        eb.WithImageUrl("https://stream.data.hu/get/13387017/olel0.gif");
                        eb.WithColor(Discord.Color.DarkGreen);
                        eb.WithFooter(footer => footer.Text = $"Nem említetted meg, hogy kit szeretnél megölelni, ezért én megöleltelek téged <3");
                        await context.Channel.SendMessageAsync("", false, eb.Build());
                    }
                }
                if (parancs == ".megver")
                {

                try
                {
                    masodikresz = tomb[1];
                    if (masodikresz.StartsWith("<@"))
                    {
                        Random random = new Random();
                        replacement = masodikresz.Replace("<@", "<@!");
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.AddField("Megverve! :rage: ", $"{context.Message.Author.Mention} megverte {masodikresz}-t!");
                        eb.WithColor(Discord.Color.DarkGreen);
                        eb.WithImageUrl("https://c.tenor.com/NcdgzWMhZXkAAAAC/spank-playful.gif");
                        await context.Channel.SendMessageAsync("", false, eb.Build());
                    }
                }
                catch
                {

                }

                }
                if (parancs == ".szólánc")
                {
                    ulong szerverid = context.Guild.Id;
                    bool vanilyenszerver = false;
                    bool vanszolanc = false;
                    masodikresz = tomb[1];
                    if (masodikresz == "töröl")
                    {
                        //csatornaid = File.ReadAllText($@"SZERVEREK\{szerverid}\szolanc.txt");
                        EmbedBuilder eb = new EmbedBuilder();
                        File.Delete($@"SZERVEREK\{szerverid}\szolanc.txt");
                        eb.AddField($"Szólánc törlése:", $"Sikeresen töröltem, mostmár létrehozhatsz új szóláncot!\n(Kérlek töröld kézileg a csatornát, és bocsánat!!)");
                        eb.WithColor(Discord.Color.Green);
                        avatar = context.Message.Author.GetAvatarUrl();
                        eb.WithAuthor(context.Message.Author.Username, avatar);
                        await context.Channel.SendMessageAsync("", false, eb.Build());
                    }
                    else
                    {
                        string dir = $@"SZERVEREK\{szerverid}";
                        if (!Directory.Exists(dir))
                        {
                            vanilyenszerver = false;
                        }
                        else
                        {
                            vanilyenszerver = true;
                        }
                        if (vanilyenszerver == false)
                        {
                            Directory.CreateDirectory(dir);
                        }
                        string path = $@"SZERVEREK\{szerverid}\szolanc.txt";
                        vanszolanc = File.Exists(path);
                        if (vanilyenszerver == true && vanszolanc == true)
                        {
                            //string szolancneve = File.ReadAllText($@"SZERVER{szerverid}\szolanc.txt");
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.AddField($"Hiba!", $"Ennek a szervernek van már szólánc csatornája!\nHa törölni szeretnéd, akkor használd a **.szólánc töröl** parancsot!");
                            eb.WithColor(Discord.Color.Red);
                            avatar = context.Message.Author.GetAvatarUrl();
                            eb.WithAuthor(context.Message.Author.Username, avatar);
                            await context.Channel.SendMessageAsync("", false, eb.Build());
                        }
                        if (vanilyenszerver == true && vanszolanc == false)
                        {
                            Random random = new Random();
                            replacement = szoveg.Replace(".szólánc ", "");
                            replacemente = replacement.Replace(" ", "-");
                            var szavak = new List<string>()
                        {
                         "csukló","fizika","tiszta","fűrész","egy","jég","eper","igen","vékony","ebéd","váll","kígyó","ing","futás","kalauz","majom","vese","mozi","vállfa","szeret","épület","kecske","padlás","szikla","nyolc","férj","barna","vasárnap","rizs","ceruza","hajó","kerékpár","gitár","ügyvéd","templom","belül","ugrik","vicces","ülés","kávé","fontos","falu","kabát","drága","monitor","bors","medve","macska","kutya","táncol","ebédel"
                        };
                            string kezdoszo = szavak[random.Next(0, szavak.Count)];
                            var channelid = await context.Guild.CreateTextChannelAsync($"{replacemente}");
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.AddField($"Sikeresen létrehoztam a szólánc csatornát.", $"\nA csatorna neve: **{replacemente}**\nA kezdő szó: **{kezdoszo}**", false);
                            eb.WithColor(Discord.Color.Green);
                            avatar = context.Message.Author.GetAvatarUrl();
                            eb.WithAuthor(context.Message.Author.Username, avatar);
                            await context.Channel.SendMessageAsync("", false, eb.Build());
                            File.WriteAllText($@"SZERVEREK\{szerverid}\szolanc.txt", $"{replacemente}");
                        }
                        if (vanilyenszerver == false && vanszolanc == false)
                        {
                            Random random = new Random();
                            replacement = szoveg.Replace(".szólánc ", "");
                            replacemente = replacement.Replace(" ", "-");
                            var szavak = new List<string>()
                        {
                         "csukló","fizika","tiszta","fűrész","egy","jég","eper","igen","vékony","ebéd","váll","kígyó","ing","futás","kalauz","majom","vese","mozi","vállfa","szeret","épület","kecske","padlás","szikla","nyolc","férj","barna","vasárnap","rizs","ceruza","hajó","kerékpár","gitár","ügyvéd","templom","belül","ugrik","vicces","ülés","kávé","fontos","falu","kabát","drága","monitor","bors","medve","macska","kutya","táncol","ebédel"
                        };
                            string kezdoszo = szavak[random.Next(0, szavak.Count)];
                            var channelid = await context.Guild.CreateTextChannelAsync($"{replacemente}");
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.AddField($"Sikeresen létrehoztam a szólánc csatornát.", $"\nA csatorna neve: **{replacemente}**\nA kezdő szó: **{kezdoszo}**", false);
                            eb.WithColor(Discord.Color.Green);
                            avatar = context.Message.Author.GetAvatarUrl();
                            eb.WithAuthor(context.Message.Author.Username, avatar);
                            await context.Channel.SendMessageAsync("", false, eb.Build());
                            File.WriteAllText($@"SZERVEREK\{szerverid}\szolanc.txt", $"{replacemente}");
                            }
                    }
                }
                #region bj 
            if (parancs == ".bj1")
            {
                masodikresz = tomb[1];
                        int penz = 0;
                        string path = $@"Bankok\{context.User.Id}bankja.txt";
                        bool bankzarva = false;
                        van = File.Exists(path);
                        string bjpath = $@"Jatekok\{context.User.Id}bj.txt";
                        bool vanbj = false;
                        van = File.Exists(path);
                        vanbj = File.Exists(bjpath);
                        if (van == true)
                        {
                            foreach (var item in File.ReadAllLines($@"Bankok\{context.User.Id}bankja.txt"))
                            {
                                adatok Adat = new adatok(item);
                                lista.Add(Adat);
                            }
                            foreach (var item in lista)
                            {
                                if (item.allapot == "zárva")
                                {
                                    bankzarva = true;
                                }
                                else
                                {
                                    bankzarva = false;
                                    penz = item.penz;
                                }
                            }
                            if(bankzarva == true)
                            {
                                EmbedBuilder eb = new EmbedBuilder();
                                eb.WithColor(Discord.Color.Green);
                                eb.AddField("Blackjack", "Hiba! A fiókod zárva van.\nHa szeretnéd kinyitni, akkor használd a **.bank kinyit** parancsot.", false);
                                eb.WithColor(Discord.Color.Red);
                                avatar = context.Message.Author.GetAvatarUrl();
                                eb.WithAuthor(context.User.Username, avatar);
                                await arg.Channel.SendMessageAsync("", false, eb.Build());
                            }
                            if(vanbj == true)
                            {
                        string id = "";
                            string sajatkartya1read = "";
                            string sajatkartya2read = "";
                            int sajatertekread = 0;
                            string dealerkartya1read = "";
                            string dealerkartya2read = "";
                            int dealerertekread = 0;
                        int bjertek = 0;
                            foreach (var item in File.ReadAllLines($@"Jatekok\{context.User.Id}bj.txt"))
                            {
                                        blackjack Adat = new blackjack(item);
                                        bjlista.Add(Adat);
                            }
                            foreach (var item in bjlista)
                            {
                            id = item.id;
                            sajatkartya1read = item.kartya1;
                            sajatkartya2read = item.kartya2;
                            sajatertekread = item.sajatertek;
                            dealerkartya1read = item.dealer1;
                            dealerkartya2read = item.dealer2;
                            dealerertekread = item.dealerertek;
                            bjertek = item.tet;
                            }
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.AddField("Blackjack\n\n", $"\nBetöltöttem az asztalod!\n\nA tét: {bjertek} :coin: \n\nA lapjaid:\n[**{sajatkartya1read}**] - [**{sajatkartya2read}**] = **{sajatertekread}**\n\nAz osztó lapjai:\n[**{dealerkartya1read}**] - [**?**]");
                        eb.WithColor(Discord.Color.DarkTeal);
                        avatar = context.Message.Author.GetAvatarUrl();
                        eb.WithAuthor(context.User.Username, avatar);
                        var gombok = new ComponentBuilder().WithButton("🎯 Húzás", "huzas", style: ButtonStyle.Secondary).WithButton("⛔ Megáll", "megall", style: ButtonStyle.Secondary);
                        await context.Channel.SendMessageAsync("", false, eb.Build(), components: gombok.Build());
                        context.Client.ButtonExecuted += betoltottbj;
                    }
                            else
                            {
                                if(masodikresz == "0")
                                {
                                    EmbedBuilder eb = new EmbedBuilder();
                                    eb.WithColor(Discord.Color.Green);
                                    eb.AddField("Blackjack", "Hiba! A tét minimum 1 lehet!", false);
                                    eb.WithColor(Discord.Color.Red);
                                    avatar = context.Message.Author.GetAvatarUrl();
                                    eb.WithAuthor(context.User.Username, avatar);
                                    await arg.Channel.SendMessageAsync("", false, eb.Build());
                                }
                                else if(int.Parse(masodikresz) > penz)
                                {
                                    EmbedBuilder eb = new EmbedBuilder();
                                    eb.WithColor(Discord.Color.Green);
                                    eb.AddField("Blackjack", "Hiba! Nincs elég kredited!", false);
                                    eb.WithColor(Discord.Color.Red);
                                    avatar = context.Message.Author.GetAvatarUrl();
                                    eb.WithAuthor(context.User.Username, avatar);
                                    await arg.Channel.SendMessageAsync("", false, eb.Build());
                                }
                                else
                                {   
                                    var lapok = new List<string>()
                                    { "1","2","3","4","5","6","7","8","9","10","J","Q","K","A"};
                                    Random random = new Random();
                                    string dealerkartya1 = lapok[random.Next(0, lapok.Count)];
                                    string dealerkartya2 = lapok[random.Next(0, lapok.Count)];
                                    string sajatkartya1 = lapok[random.Next(0, lapok.Count)];
                                    string sajatkartya2 = lapok[random.Next(0, lapok.Count)];
                                    int dealerertek1 = 0;
                                    int dealerertek2 = 0;
                                    int sajatertek1 = 0;
                                    int sajatertek2 = 0;
                                    bool usernyert = false;
                                    bool dealernyert = false;
                                    if(dealerkartya1 == "J")
                                    {
                                        dealerertek1 = 10;
                                    }
                                    if (dealerkartya1 == "Q")
                                    {
                                        dealerertek1 = 10;
                                    }
                                    if (dealerkartya1 == "K")
                                    {
                                        dealerertek1 = 10;
                                    }
                                    if (dealerkartya1 == "A")
                                    {
                                        dealerertek1 = 10;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            dealerertek1 = int.Parse(dealerkartya1);
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    if (dealerkartya2 == "J")
                                    {
                                        dealerertek2 = 10;
                                    }
                                    if (dealerkartya2 == "Q")
                                    {
                                        dealerertek2 = 10;
                                    }
                                    if (dealerkartya2 == "K")
                                    {
                                        dealerertek2 = 10;
                                    }
                                    if (dealerkartya2 == "A")
                                    {
                                        dealerertek2 = 10;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            dealerertek2 = int.Parse(dealerkartya2);
                                        }
                                        catch
                                        {

                                        }
                                    }
                             if (sajatkartya1 == "J")
                            {
                                sajatertek1 = 10;
                            }
                            if (sajatkartya1 == "Q")
                            {
                                sajatertek1 = 10;
                            }
                            if (sajatkartya1 == "K")
                            {
                                sajatertek1 = 10;
                            }
                            if (sajatkartya1 == "A")
                            {
                                sajatertek1 = 10;
                            }
                            else
                            {
                                try
                                {
                                    sajatertek1 = int.Parse(sajatkartya1);
                                }
                                catch
                                {

                                }
                            }
                            if (sajatkartya2 == "J")
                            {
                                sajatertek2 = 10;
                            }
                            if (sajatkartya2 == "Q")
                            {
                                sajatertek2 = 10;
                            }
                            if (sajatkartya2 == "K")
                            {
                                sajatertek2 = 10;
                            }
                            if (sajatkartya2 == "A")
                            {
                                sajatertek2 = 10;
                            }
                            else
                            {
                                try
                                {
                                    sajatertek2 = int.Parse(sajatkartya2);
                                }
                                catch
                                {

                                }
                            }
                            int sajatertek = sajatertek1 + sajatertek2;
                            int dealerertek = dealerertek1 + dealerertek2;
                            File.WriteAllText($@"Jatekok\{context.User.Id}bj.txt", $"{context.User.Id};{masodikresz};{sajatkartya1};{sajatertek1};{sajatkartya2};{sajatertek2};kartya3;0;kartya4;0;kartya5;0;kartya6;0;kartya7;0;{sajatertek};{dealerkartya1};{dealerertek1};{dealerkartya2};{dealerertek2};{dealerertek};0");
                            if (sajatkartya1 == "A" && sajatkartya2 == "J" || sajatkartya1 == "J" && sajatkartya2 == "A")
                            {
                                usernyert = true;
                                sajatertek = 21;
                            }
                            if (usernyert == false && dealernyert == false)
                            {
                                EmbedBuilder eb = new EmbedBuilder();
                                await context.Channel.SendMessageAsync("Szia! A blackjack még fejlesztés/tesztelés alatt van. Ha valami hibát tapasztalsz, akkor kérlek itt jelezd: https://docs.google.com/forms/d/e/1FAIpQLScsUUtnhRanoly6E1lG-gnqQ_sPMbVwXymAidNUN-jWzrNfLA/viewform?usp=pp_url");
                                eb.AddField("Blackjack\n\n", $"A tét: {masodikresz} :coin: \n\nA lapjaid:\n[**{sajatkartya1}**] - [**{sajatkartya2}**] = **{sajatertek}**\n\nAz osztó lapjai:\n[**{dealerkartya1}**] - [**?**]");
                                eb.WithColor(Discord.Color.DarkTeal);
                                avatar = context.Message.Author.GetAvatarUrl();
                                eb.WithAuthor(context.User.Username, avatar);
                                var gombok = new ComponentBuilder().WithButton("🎯 Húzás", "huzas",style:ButtonStyle.Secondary).WithButton("⛔ Megáll", "megall", style:ButtonStyle.Secondary);
                                await context.Channel.SendMessageAsync("", false, eb.Build(), components:gombok.Build());
                                context.Client.ButtonExecuted += betoltottbj;
                            }
                            if (usernyert == true && dealernyert == false)
                            {
                                EmbedBuilder eb = new EmbedBuilder();
                                eb.AddField("Blackjack - Nyertél!\n\n", $"A tét: {masodikresz} :coin: \n\nA lapjaid:\n[**{sajatkartya1}**] - [**{sajatkartya2}**] = **{sajatertek}**\n\nAz osztó lapjai:\n[**{dealerkartya1}**] - [**{dealerkartya2}**] = **{dealerertek}**\n\nProfit: **{masodikresz}** :coin:");
                                eb.WithColor(Discord.Color.DarkTeal);
                                avatar = context.Message.Author.GetAvatarUrl();
                                eb.WithAuthor(context.User.Username, avatar);
                                await context.Channel.SendMessageAsync("", false, eb.Build());
                            }
                        }
                        }
                    }
                        else
                        {
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.WithColor(Discord.Color.Green);
                            eb.AddField("Blackjack", "Hiba! Neked még nincs fiókod!\nHasználd a **.bank létrehoz** parancsot, hogy készíts egyet.", false);
                            eb.WithColor(Discord.Color.Red);
                            avatar = context.Message.Author.GetAvatarUrl();
                            eb.WithAuthor(context.User.Username, avatar);
                            await arg.Channel.SendMessageAsync("", false, eb.Build());
                        }
                    }
                    
            #endregion
                if(parancs == ".bj")
                {
                string path = $@"Bankok\{context.User.Id}bankja.txt";
                van = File.Exists(path);
                bool zarva = false;
                int penz = 0;
                if(van == true)
                {
                    foreach (var item in File.ReadAllLines($@"Bankok\{context.User.Id}bankja.txt"))
                    {
                        adatok Adat = new adatok(item);
                        lista.Add(Adat);
                    }
                    foreach (var item in lista)
                    {
                        if (item.allapot == "zárva")
                        {
                            zarva = true;
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.AddField("Blackjack:", $"A fiókod zárva van. Kinyithatod a **.bank kinyit** paranccsal.");
                            eb.WithColor(Discord.Color.Red);
                            avatar = context.Message.Author.GetAvatarUrl();
                            eb.WithAuthor(context.Message.Author.Username, avatar);
                            await context.Channel.SendMessageAsync("", false, eb.Build());
                            break;
                        }
                        else
                        {
                            zarva = false;
                            penz = item.penz;
                        }
                    }
                    if (penz >= 200 && zarva == false)
                    {
                        Random random = new Random();
                        int osztoszam = random.Next(8, 21);
                        int sajatszam1 = random.Next(1, 22);
                        int sajatszam2 = random.Next(1, 22);
                        int sajatszam3 = random.Next(1, 22);
                        int sajatszam4 = random.Next(1, 22);
                        int nyerhet = random.Next(1, 35);
                        if(nyerhet > 10)
                        {
                            osztoszam = random.Next(8, 21);
                            sajatszam1 = random.Next(1, 12);
                            sajatszam2 = random.Next(1, 13);
                            sajatszam3 = random.Next(1, 14);
                            sajatszam4 = random.Next(1, 12);
                        }
                        else
                        {
                            osztoszam = random.Next(8, 21);
                            sajatszam1 = random.Next(1, 22);
                            sajatszam2 = random.Next(1, 22);
                            sajatszam3 = random.Next(1, 22);
                            sajatszam4 = random.Next(1, 22);
                        }
                        var nyeremenyek = new List<string>()
                        { "200","200","200","300","500","750","750","1000","1500","2000","2500","3000","3500","4000"};
                        string nyeremeny = nyeremenyek[random.Next(0, nyeremenyek.Count())];
                        if(nyeremeny == "1500" || nyeremeny == "2000" || nyeremeny == "2500" || nyeremeny == "3000" || nyeremeny == "3500" || nyeremeny == "4000")
                        {
                            osztoszam = random.Next(17, 21);
                        }
                        if (sajatszam1 > osztoszam || sajatszam2 > osztoszam || sajatszam3 > osztoszam || sajatszam4 > osztoszam)
                        {
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.AddField("Blackjack:", $"Ha az osztó számánál nagyobb számot találsz, akkor nyertél!\n\n‎Az osztó száma: ||‎ ‎ ‎ ‎ ‎ {osztoszam}‎ ‎ ‎ ‎ ‎ ||\n\nNyeremény: ||‎ ‎ ‎ ‎ ‎ {nyeremeny}‎ ‎ ‎ ‎ ||‎:coin: \n\nA számaid:\n||‎‎ ‎ ‎ ‎ ‎ {sajatszam1}‎‎ ‎ ‎ ‎ ‎ ||‎ ‎ ‎ ||‎ ‎ ‎ ‎ ‎ {sajatszam2}‎ ‎ ‎ ‎ ‎ ||‎ ‎ ‎ ||‎ ‎ ‎ ‎ ‎ {sajatszam3}‎ ‎ ‎ ‎ ‎ ||‎ ‎ ‎ ||‎ ‎ ‎ ‎ ‎ {sajatszam4}‎ ‎ ‎ ‎ ‎ ||");
                            eb.WithColor(Discord.Color.Teal);
                            avatar = context.Message.Author.GetAvatarUrl();
                            eb.WithAuthor(context.Message.Author.Username, avatar);
                            eb.WithFooter(footer => footer.Text = $"Játék ára: 200 kredit");
                            await context.Channel.SendMessageAsync("", false, eb.Build());
                            int sajatpenzlesz = penz + int.Parse(nyeremeny) - 200;
                            string text = File.ReadAllText($@"Bankok\{context.User.Id}bankja.txt");
                            text = text.Replace($"{penz}", $"{sajatpenzlesz}");
                            File.WriteAllText($@"Bankok\{context.User.Id}bankja.txt", text);
                        }
                        else
                        {
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.AddField("Blackjack:", $"Ha az osztó számánál nagyobb számot találsz, akkor nyertél!\n\n‎Az osztó száma: ||‎ ‎ ‎ ‎ ‎ {osztoszam}‎ ‎ ‎ ‎ ‎ ||\n\nNyeremény: ||‎ ‎ ‎ ‎ ‎ {nyeremeny}‎ ‎ ‎ ‎ ||‎:coin: \n\nA számaid:\n||‎‎ ‎ ‎ ‎ ‎ {sajatszam1}‎‎ ‎ ‎ ‎ ‎ ||‎ ‎ ‎ ||‎ ‎ ‎ ‎ ‎ {sajatszam2}‎ ‎ ‎ ‎ ‎ ||‎ ‎ ‎ ||‎ ‎ ‎ ‎ ‎ {sajatszam3}‎ ‎ ‎ ‎ ‎ ||‎ ‎ ‎ ||‎ ‎ ‎ ‎ ‎ {sajatszam4}‎ ‎ ‎ ‎ ‎ ||");
                            eb.WithColor(Discord.Color.Teal);
                            avatar = context.Message.Author.GetAvatarUrl();
                            eb.WithAuthor(context.Message.Author.Username, avatar);
                            eb.WithFooter(footer => footer.Text = $"Játék ára: 200 kredit");
                            await context.Channel.SendMessageAsync("", false, eb.Build());
                            int sajatpenzlesz = penz - 200;
                            string text = File.ReadAllText($@"Bankok\{context.User.Id}bankja.txt");
                            text = text.Replace($"{penz}", $"{sajatpenzlesz}");
                            File.WriteAllText($@"Bankok\{context.User.Id}bankja.txt", text);
                        }
                    }
                    else
                    {
                        if (zarva == false)
                        {
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.AddField("Blackjack:", $"Nincs elég kredited! Egy játék ára 200 :coin: kredit.");
                            eb.WithColor(Discord.Color.Red);
                            avatar = context.Message.Author.GetAvatarUrl();
                            eb.WithAuthor(context.Message.Author.Username, avatar);
                            await context.Channel.SendMessageAsync("", false, eb.Build());
                        }
                    }
                }
                else
                {
                    EmbedBuilder eb = new EmbedBuilder();
                    eb.AddField("Blackjack:", $"Neked még nincs fiókod.\nLétrehozhatsz egyet a **.bank létrehoz** paranccsal.");
                    eb.WithColor(Discord.Color.Red);
                    avatar = context.Message.Author.GetAvatarUrl();
                    eb.WithAuthor(context.Message.Author.Username, avatar);
                    await context.Channel.SendMessageAsync("", false, eb.Build());
                }
            }
                lista.Clear();
                if(parancs == ".play")
                {
                string path = tomb[1];
                string utvonal = $@"Hangeffektek\{path}.mp3";
                if (!File.Exists(utvonal))
                {
                    EmbedBuilder eb = new EmbedBuilder();
                    eb.WithTitle("Ezt a hangeffektett nem találom!\n Az elérhető hangok elérhetőek a: /hangok parancssal ");
                    avatar = context.Message.Author.GetAvatarUrl();
                    eb.WithAuthor(context.Message.Author.Username, avatar);
                    eb.WithColor(Discord.Color.Red);
                    await context.Channel.SendMessageAsync("", false, eb.Build());
                    return;
                }
                else
                {
                    EmbedBuilder eb = new EmbedBuilder();
                    eb.WithTitle($"Lejátszotta ezt a hangeffektett: ``{path}``");
                    avatar = context.Message.Author.GetAvatarUrl();
                    eb.WithAuthor(context.Message.Author.Username, avatar);
                    eb.WithColor(Discord.Color.Blue);
                    await context.Channel.SendMessageAsync("", false, eb.Build());
                }
                }
                if(parancs == "botsay")
                {
                replacement = szoveg.Replace("botsay", "");
                _ = context.Message.DeleteAsync();
                await context.Channel.SendMessageAsync($"{replacement}");
                }
                ///////////////////////////////////
                ///////////////////////////////////
            }
        public async Task betoltottbj(SocketMessageComponent arg)
        {
            switch (arg.Data.CustomId)
            {
                case "huzas":
                    await arg.RespondAsync($"{arg.User.Mention}");
                    break;
                case "megall":
                    await arg.RespondAsync($"{arg.User.Mention}");
                    int tet = 0;
                    string sajatkartya1 = "";
                    string sajatkartya2 = "";
                    string sajatkartya3 = "";
                    string sajatkartya4 = "";
                    string sajatkartya5 = "";
                    string sajatkartya6 = "";
                    string sajatkartya7 = "";
                    int sajatossz = 0;
                    int kartya1ertek = 0;
                    int kartya2ertek = 0;
                    int kartya3ertek = 0;
                    int kartya4ertek = 0;
                    int kartya5ertek = 0;
                    int kartya6ertek = 0;
                    int kartya7ertek = 0;
                    string dealer1 = "";
                    string dealer2 = "";
                    int dealerertek = 0;
                    int huzasokszama = 0;
                    foreach (var item in File.ReadAllLines($@"Jatekok\{arg.User.Id}bj.txt"))
                    {
                        blackjack Adat = new blackjack(item);
                        bjlista.Add(Adat);
                    }
                    foreach (var item in bjlista)
                    {
                        tet = item.tet;
                        sajatkartya1 = item.kartya1;
                        sajatkartya2 = item.kartya2;
                        sajatkartya3 = item.kartya3;
                        sajatkartya4 = item.kartya4;
                        sajatkartya5 = item.kartya5;
                        sajatkartya6 = item.kartya6;
                        sajatkartya7 = item.kartya7;
                        sajatossz = item.sajatertek;
                        kartya1ertek = item.kartya1ertek;
                        kartya2ertek = item.kartya2ertek;
                        kartya3ertek = item.kartya3ertek;
                        kartya4ertek = item.kartya4ertek;
                        kartya5ertek = item.kartya5ertek;
                        kartya6ertek = item.kartya6ertek;
                        kartya7ertek = item.kartya7ertek;
                        dealer1 = item.dealer1;
                        dealer2 = item.dealer2;
                        dealerertek = item.dealerertek;
                        huzasokszama = item.huzasokszama;
                    }
                    var lapok = new List<string>()
                    { "1","2","3","4","5","6","7","8","9","10","J","Q","K","A"};
                    Random random = new Random();
                    string dealerkartya3 = "";
                    string dealerkartya4 = "";
                    string dealerkartya5 = "";
                    string dealerkartya6 = "";
                    string dealerkartya7 = "";
                    if (huzasokszama == 0)
                    {
                        while(dealerertek < sajatossz && dealerertek < 21)
                        {
                            dealerkartya3 = lapok[random.Next(0, lapok.Count)];
                            if (dealerkartya3 == "J")
                            {
                                dealerertek += 10;
                            }
                            if (dealerkartya3 == "Q")
                            {
                                dealerertek += 10;
                            }
                            if (dealerkartya3 == "K")
                            {
                                dealerertek += 10;
                            }
                            if (dealerkartya3 == "A")
                            {
                                dealerertek += 10;
                            }
                            else
                            {
                                try
                                {
                                    dealerertek = int.Parse(dealerkartya3);
                                }
                                catch
                                {

                                }
                            }
                            dealerkartya4 = lapok[random.Next(0, lapok.Count)];
                            if (dealerkartya4 == "J")
                            {
                                dealerertek += 10;
                            }
                            if (dealerkartya4 == "Q")
                            {
                                dealerertek += 10;
                            }
                            if (dealerkartya4 == "K")
                            {
                                dealerertek += 10;
                            }
                            if (dealerkartya4 == "A")
                            {
                                dealerertek += 10;
                            }
                            else
                            {
                                try
                                {
                                    dealerertek = int.Parse(dealerkartya4);
                                }
                                catch
                                {

                                }
                            }
                            dealerkartya5 = lapok[random.Next(0, lapok.Count)];
                            if (dealerkartya5 == "J")
                            {
                                dealerertek += 10;
                            }
                            if (dealerkartya5 == "Q")
                            {
                                dealerertek += 10;
                            }
                            if (dealerkartya5 == "K")
                            {
                                dealerertek += 10;
                            }
                            if (dealerkartya5 == "A")
                            {
                                dealerertek += 10;
                            }
                            else
                            {
                                try
                                {
                                    dealerertek = int.Parse(dealerkartya5);
                                }
                                catch
                                {

                                }
                            }
                            dealerkartya6 = lapok[random.Next(0, lapok.Count)];
                            if (dealerkartya6 == "J")
                            {
                                dealerertek += 10;
                            }
                            if (dealerkartya6 == "Q")
                            {
                                dealerertek += 10;
                            }
                            if (dealerkartya6 == "K")
                            {
                                dealerertek += 10;
                            }
                            if (dealerkartya6 == "A")
                            {
                                dealerertek += 10;
                            }
                            else
                            {
                                try
                                {
                                    dealerertek = int.Parse(dealerkartya6);
                                }
                                catch
                                {

                                }
                            }
                            dealerkartya7 = lapok[random.Next(0, lapok.Count)];
                            if (dealerkartya7 == "J")
                            {
                                dealerertek += 10;
                            }
                            if (dealerkartya7 == "Q")
                            {
                                dealerertek += 10;
                            }
                            if (dealerkartya7 == "K")
                            {
                                dealerertek += 10;
                            }
                            if (dealerkartya7 == "A")
                            {
                                dealerertek += 10;
                            }
                            else
                            {
                                try
                                {
                                    dealerertek = int.Parse(dealerkartya7);
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    if (dealerertek > sajatossz && dealerertek < 21)
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.AddField("Blackjack - Vesztettél!\n\n", $"A tét: {tet} :coin: \n\nA lapjaid:\n[**{sajatkartya1}**] - [**{sajatkartya2}**] = **{sajatossz}**\n\nAz osztó lapjai:\n[**{dealer1}**] - [**{dealer2}**] = **{dealerertek}**\nEnnyit buktál: **{tet}** :coin:");
                        eb.WithColor(Discord.Color.Red);
                        await arg.Channel.SendMessageAsync("", false, eb.Build());
                        File.Delete($@"Jatekok\{arg.User.Id}bj.txt");
                    }
                    break;
            }
        }
        public class Result
        {
            public Query query { get; set; }
        }

        public class Query
        {
            public Dictionary<string, Page> pages { get; set; }
        }

        public class Page
        {
            public string extract { get; set; }
        }
        private void statusztimertick(object source, ElapsedEventArgs e)
        {
            i++;
            if (i == 1)
            {
                kliens.SetGameAsync($"{kliens.Guilds.Count} szerver .parancsok", "", ActivityType.Watching);
            }
            if (i == 2)
            {
                kliens.SetGameAsync("Koronavírus .covid", "", ActivityType.Watching);
            }
            if (i == 3)
            {
                kliens.SetGameAsync("Időjárás .idő", "", ActivityType.Watching);
            }
            if (i == 4)
            {
                kliens.SetGameAsync("Elakadtál? .parancsok", "", ActivityType.Watching);
            }
           if(i == 5)
            {
                kliens.SetGameAsync("Wikipédia .wiki", "", ActivityType.Watching);
            }
            if (i == 6)
            {
                i = 0;
            }
        }
    }
}
