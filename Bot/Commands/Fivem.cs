using Bot.Model;
using Bot.Service;
using Discord;
using Discord.Audio;
using Discord.Commands;
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
    public class Fivem : ModuleBase<SocketCommandContext>
    {

        private readonly WebApiService _webApiService = new WebApiService();
        private readonly SaverService _saverService = new SaverService();
        private static string joinServerUrl = "https://madsword.site/loginapi/joinserver.php";
        private static string closeServerUrl = "https://madsword.site/loginapi/joinserver.php";
        private static string serverIpFinderUrl = "https://madsword.site/launcherapi/launcher.php?veri=ip";
        private static string serverPortFinderUrl = "https://madsword.site/launcherapi/launcher.php?veri=port";

        [Command("giris")]
        public async Task FivemPlay([Remainder]Int64 id = 0)
        {
            var model = _saverService.Get(Context.Message.Author.Id);
            if (id > 0)
            {
                model.Hex = "steam:" + id.ToString("x");
                _saverService.Update(model);
            }
            else if (id == 0 && model.Hex == null)
            {
                await ReplyAsync("Mevcut kayıt bulunamadı, lütfen SteamID ile kayıt olun. " + Context.Message.Author.Mention);
                return;
            }

            var values = new Dictionary<string, string>();
            values.Add("steam_name", "Oyuncu");
            values.Add("steam_hex", model.Hex);
            values.Add("ip", "127.0.0.1");
            values.Add("server", "vanaheimrp");
            values.Add("tarih", "04.06.2002 21:21");
            var canPlay = await _webApiService.GetPostAsync(joinServerUrl, values);
            if (canPlay)
            {
                await ReplyAsync(model.Hex + " Oyuna girebilirsin! " + Context.Message.Author.Mention);
            }
            else
            {
                await ReplyAsync("Bir hata meydana geldi! " + Context.Message.Author.Mention);
            }
        }
        [Command("cikis")]
        public async Task FivemQuit([Remainder]Int64 id = 0)
        {
            var model = _saverService.Get(Context.Message.Author.Id);
            if (id > 0)
            {
                model.Hex = "steam:" + id.ToString("x");
                _saverService.Update(model);
            }
            else if (id == 0 && model.Hex == null)
            {
                await ReplyAsync("Mevcut kayıt bulunamadı, lütfen SteamID ile kayıt olun. " + Context.Message.Author.Mention);
                return;
            }
            var values = new Dictionary<string, string>();
            values.Add("steam_hex", model.Hex);
            values.Add("server", "vanaheimrp");
            var canPlay = await _webApiService.GetPostAsync(closeServerUrl, values);
            if (canPlay)
            {
                await ReplyAsync(model.Hex + " Oyundan çıkışın alındı ;) " + Context.Message.Author.Mention);
            }
            else
            {
                await ReplyAsync("Bir hata meydana geldi! " + Context.Message.Author.Mention);
            }
        }
        [Command("ip")]
        public async Task FivemIp()
        {
            await ReplyAsync("31.214.243.212:30120 " + Context.Message.Author.Mention);
        }
        [Command("ipbul")]
        public async Task FivemIp(string servername)
        {
            var values = new Dictionary<string, string>();
            values.Add("server", servername);
            var ip = await _webApiService.GetPostAsync(serverIpFinderUrl, values, "127.0.0.1");
            values = new Dictionary<string, string>();
            values.Add("server", servername);
            var port = await _webApiService.GetPostAsync(serverPortFinderUrl, values,"127.0.0.1");
            if (ip != null && port != null)
            {
                try
                {
                    int.Parse(port);
                    await ReplyAsync(ip + ":" + port+ " " + Context.Message.Author.Mention);
                }
                catch
                {
                    await ReplyAsync("Sunucu Bulunamadı! " + Context.Message.Author.Mention);
                }
            }
            else
            {
                await ReplyAsync("Sunucu Bulunamadı! " + Context.Message.Author.Mention);
            }
        }
    }
}
