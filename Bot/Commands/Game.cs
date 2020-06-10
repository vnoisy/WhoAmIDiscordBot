using Bot.Model;
using Bot.Service;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Commands
{
    public class Game : ModuleBase<SocketCommandContext>
    {

        private readonly WebApiService api = new WebApiService();

        [Command("play")]
        public async Task SayAsync()
        {
            IVoiceChannel chId = (Context.User as IVoiceState).VoiceChannel;
            if (chId == null)
            {
                await ReplyAsync("Oyunu başlatmak için bir odada olman gerekir! " + Context.Message.Author.Mention);
                return;
            }
            var channel = Context.Client.GetChannel(chId.Id);
            var users = channel.Users.Where(x => !x.IsBot);
            var data = Program.game.FirstOrDefault(x => x.ChannelId == chId.Id);
            if (data != null && data.ChannelId != 0)
            {
                await ReplyAsync("Bu kanalda daha önce bir oyun oluşturulmuş ve hala devam ediyor, Lütfen " + data.User.Mention + " ulaşın.");
                return;
            }
            await ReplyAsync(users.Count() + " Oyuncu bulundu, oyun oluşturuluyor...");
            Thread.Sleep(150);
            await ReplyAsync("Şampiyonlar alınıyor..");
            Thread.Sleep(300);
            var champs = api.Get<List<ChampionModel>>("https://raw.githubusercontent.com/vnoisy/WhoAmIDiscordBot/master/game.json");

            await ReplyAsync("Oyunculara özel şampiyonlar atanıyor..");
            var random = new Random();
            var players = new List<Players>();

            foreach (var item in users)
            {

                var guildUser = (item as SocketGuildUser);
                if (guildUser.IsDeafened || guildUser.IsMuted || guildUser.IsSelfMuted || guildUser.IsSelfDeafened)
                {
                    await ReplyAsync(item.Mention + " Oyuna katılamazsınız Mute veya Deafened moddasınız!");
                }
                else
                {
                    var randomNum = randomGen(champs.Count(), players);
                    players.Add(new Players
                    {
                        id = randomNum,
                        Champion = champs.FirstOrDefault(x => x.id == randomNum),
                        Player = item,
                    });
                    Thread.Sleep(150);
                }
            }
            await ReplyAsync("Oyunculara şampiyon listeleri gönderiliyor..");
            var order = String.Join(",", players.OrderBy(x => x.id).Select(x => x.Player.Username).ToArray());
            foreach (var item in players)
            {
                Thread.Sleep(150);
                var friendsChamp = players.Where(x => x.id != item.Champion.id);
                var message = "Oyun " + Context.User.Mention + " tarafından `" + Context.Guild.VoiceChannels.FirstOrDefault(x => x.Id == channel.Id).Name + "` odasında kurulmuştur. ```" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "\n";
                foreach (var c in friendsChamp)
                {
                    message += c.Player.Username + " : " + c.Champion.name + "\n";
                }
                Thread.Sleep(25);
                await item.Player.SendMessageAsync(message + "\nSıralama: " + order + "```");
            }
            Program.game.Add(new GameModel { ChannelId = chId.Id, CreatorId = Context.User.Id, User = Context.User, CreatedOn = DateTime.Now.AddMinutes(+30) });
            Console.WriteLine(Context.Guild.Name + " sunucusunda " + chId.Id + " (" + chId.Name + ") kanalına " + Context.User.Id + " (" + Context.User.Username + ") tarafından " + DateTime.Now + " tarihinde oyun oluşturuldu.");
        }
        [Command("stop")]
        public async Task StopAsync()
        {
            IVoiceChannel chId = (Context.User as IVoiceState).VoiceChannel;
            if (chId == null) { await ReplyAsync("Bu özelliği kullanmak için kanalda olman şart!"); return; }
            var data = Program.game.FirstOrDefault(x => x.CreatorId == Context.User.Id && x.ChannelId == chId.Id);
            if (data != null && data.CreatorId != 0 && data.ChannelId != 0)
            {
                await ReplyAsync("Son oluşturulan oyun silindi." + Context.User.Mention);
                Program.game.Remove(data);
            }
            else if (data != null && data.CreatedOn < DateTime.Now)
            {
                await ReplyAsync("Son oluşturulan oyun silindi." + Context.User.Mention);
                Program.game.Remove(data);
            }
            else
            {
                if (Program.game.Count > 0)
                {
                    data = Program.game.FirstOrDefault(x => x.ChannelId == chId.Id);
                    if (data != null && !UserIsInDiscord(data))
                    {
                        Program.game.Remove(data);
                        await ReplyAsync("Kanal'a ait son oyun silindi, Çünkü oyun sahibi aktif değil! " + Context.User.Mention);
                        return;
                    }
                    else if (data != null)
                    {
                        await ReplyAsync("Oyun hala devam ediyor! Oyunu " + Math.Round(data.CreatedOn.Subtract(DateTime.Now).TotalMinutes, 2) + " dakika sonra silebilirsiniz! " + Context.User.Mention);
                        return;
                    }
                }
                await ReplyAsync("Bulunduğunuz kanalda veya Adınıza kayıtlı bir oyun bulunamadı, Kanalda oyun devam ediyor olabilir! " + Context.User.Mention);
            }
        }
        int randomGen(int max, List<Players> players)
        {
            var random = new Random();
            int randomNum;
            do
            {
                randomNum = random.Next(1, max);
            }
            while (players != null && players.Count > 0 && players.Where(x => x.id == randomNum).Any());
            return randomNum;
        }
        private bool UserIsInDiscord(GameModel game)
        {
            IVoiceChannel chId = (game.User as IVoiceState).VoiceChannel;
            if (chId == null) return false;
            return true;
        }
    }
}
