using Bot.Model;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;

namespace Bot
{
    public class Program
    {
        private CommandService _commands;
        private DiscordSocketClient _client;
        private IServiceProvider _services;
        public static List<GameModel> game = new List<GameModel>();
        public static List<GuildPassedTestList> idList = new List<GuildPassedTestList>();
        private static DateTime kickTime = DateTime.Now;
        private static void Main() => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            // Avoid hard coding your token. Use an external source instead in your code.
            string token = null;

            _services = new ServiceCollection()
                .AddSingleton(_client).AddSingleton(_commands).BuildServiceProvider();

            await InstallCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived Event into our Command Handler
            _client.MessageReceived += HandleCommandAsync;

            _client.Ready += Client_Ready;
            // Discover all of the commands in this assembly and load them.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        }

        private Task Client_Ready()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 1500;
            aTimer.Enabled = true;
            return null;
        }
        private string AnyKick()
        {
            var userList = new List<ulong>();
            userList.Add(163719887586328576);
            userList.Add(172680026137821186);
            userList.Add(160897770465656832);
            userList.Add(558729595281735681);
            userList.Add(206156918777970689);
            var selectedDiscord = _client.Guilds.FirstOrDefault(x => x.Id == 274625711921430528);
            var logs = selectedDiscord.GetAuditLogsAsync(10, RequestOptions.Default, null, null, (ActionType)27).ToListAsync().Result.FirstOrDefault().Where(x => x.CreatedAt > kickTime);
            foreach (var item in logs)
            {
                if (userList.Any(x => x == item.User.Id))
                {
                    kickTime = DateTime.Now.AddSeconds(5);
                    var message = "";
                    if (item.User.Id == 163719887586328576)
                    {
                        message = $"MERT {item.User.Mention} SÜREKLİ BENİ ATIYOR \nAMK YETSKİNİ KÖTÜYE KULLANIYOR \nİYİCENE ŞIMARDI \nALANYAYA GİDİCEM GÖTÜNÜ SİKİCEM \nYETKİSİNİ AL ŞUNUN";
                    }
                    else
                    {
                        message = $"MERT {item.User.Mention} SÜREKLİ BENİ ATIYOR \nAMK YETSKİNİ KÖTÜYE KULLANIYOR \nİYİCENE ŞIMARDI \nYETKİSİNİ AL ŞUNUN";
                    }
                    selectedDiscord.Owner.SendMessageAsync(message);
                    item.User.SendMessageAsync($"YETER ARTIK ATMA MİLLETİ MERTİDE RAHATSIZ ETME ANANIN AMINA TELETABININ ANTENLERINI SOKAR GÖBEĞINDEKI TELEVIZYONDAN ULUSAL PORNO YAYINI YAPARIM ANANI ÖZGÜRLÜK HEYKELININ YANMAYAN MEŞALESINDE SIKER ŞEHRI DUMAN EDERIM HOLLYWOOD BULVARINDA DONLA GEZEN ANANIN AMINA TOPUKLU AYAKKABI SOKAYIM ANANI IKIZ KULELERININ YEDINCI KATINA CIKARTIR AMINA UÇAKLA GIRERIM");
                    return item.User.Username;
                }
            }
            return null;
        }
        private string AlpKicking()
        {
            var userList = new List<ulong>();
            userList.Add(172680026137821186);
            var selectedDiscord = _client.Guilds.FirstOrDefault(x => x.Name == "VAG");
            var logs = selectedDiscord.GetAuditLogsAsync(10, RequestOptions.Default, null, null, (ActionType)27).ToListAsync().Result.FirstOrDefault().Where(x => x.CreatedAt > kickTime);
            foreach (var item in logs)
            {
                if (userList.Any(x => x == item.User.Id))
                {
                    kickTime = DateTime.Now.AddSeconds(5);
                    foreach (var item2 in selectedDiscord.Users)
                    {
                        item2.SendMessageAsync("BENİM ADIM ALP ÇOKKOŞAR TAM BİR OROSPU EVLADIYIM, DİSCORD ÜZERİNDEN ARKADAŞLARIMI ATTIĞIM İÇİN BU MESAJ SİZE İLETİLDİ BENİM NE KADAR BÜYÜK BİR OROSPU EVLADI OLDUĞUMU GÖRMEK İÇİN LÜTFEN ADRESLERİ İNLECEYİN: \n Instagram: alpcokkosar \n ev adresim: esenyurt nazım hikmet bulv. inovia e1 daire 110 \n dip not: ben bir orospu dölüyüm. ");
                    }
                    item.User.SendMessageAsync($"şimdi yarra yedin.");
                    return item.User.Username;
                }
            }
            return null;
        }
        public class GuildChannelList
        {
            public SocketGuildChannel Channel { get; set; }
            public SocketGuild Guild { get; set; }
        }
        public class GuildPassedTestList
        {
            public SocketGuild Guild { get; set; }
            public ulong Id { get; set; }
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var guilds = _client.Guilds.Select(x => new GuildChannelList
            {
                Channel = x.Channels.FirstOrDefault(y => y.Name.ToLower().Contains("test")),
                Guild = x,
            }).Where(x => x.Channel != null).ToList();
            foreach (var guild in guilds)
            {
                if (guild.Channel.Users != null)
                {
                    foreach (var user in guild.Channel.Users)
                    {
                        if (!idList.Any(x => x.Guild.Id == guild.Guild.Id && x.Id == user.Id))
                        {
                            var added = new GuildPassedTestList
                            {
                                Guild = guild.Guild,
                                Id = user.Id,
                            }; idList.Add(added);

                            var random = new Random();
                            var n1 = random.Next(2, 11);
                            var n2 = random.Next(2, 11);
                            var result = n1 * n2;
                            Console.WriteLine($"{guild.Guild.Name} {user.Username}/{user.Mention} joining test, question: {n1} * {n2} = {result}");
                            var msg = user.SendMessageAsync($"{n1} * {n2} = ?");
                            var rmsg = user.SendMessageAsync($"you have 5 secs.");
                            rmsg.Wait();
                            msg.Wait();
                            Task.Delay(5000).Wait();
                            try
                            {

                                var dmch = user.GetOrCreateDMChannelAsync().Result.Id;
                                var dm = _client.GetDMChannelAsync(dmch).Result.GetMessagesAsync(1).ToListAsync();
                                dm.AsTask().Wait();
                                var cdm = dm.Result[1];
                                var msgx = cdm.First().Content != null ? cdm.First().Content : "";

                                if (msgx != null && result.ToString() != msgx)
                                {
                                    Console.WriteLine($"{guild.Guild.Name} {user.Username}/{user.Mention} fuckedup");
                                    user.SendMessageAsync("time is up.");
                                    user.ModifyAsync(x => x.Channel = null);
                                    idList.Remove(added);
                                }
                                else
                                {
                                    Console.WriteLine($"{guild.Guild.Name} {user.Username}/{user.Mention} passed the test!");
                                    user.SendMessageAsync("passed the test nice!");
                                }
                            }
                            catch (Exception ex)
                            {
                                idList.Remove(added);
                                Console.WriteLine($"{guild.Guild.Name} {user.Username}/{user.Mention} code was broken!");
                            }
                        }
                    }
                }
            }

            var anyKick = AnyKick();
            if (anyKick != null)
            {
                Console.WriteLine($"Using disconnect-kick perm: {anyKick} {DateTime.Now.ToString()}");
            }

        }
        private async Task ConnectAsync()
        {
            var g = _client.Guilds.FirstOrDefault(x => x.Name == "VAG").Users.FirstOrDefault(x => x.Id == 163719887586328576);
            IVoiceChannel channel = (g as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();

        }
        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message is a command, based on if it starts with '!' or a mention prefix
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
            // Create a Command Context
            var context = new SocketCommandContext(_client, message);
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed successfully)
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}
