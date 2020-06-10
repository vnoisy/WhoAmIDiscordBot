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
using System.Xml;

namespace Bot.Commands
{
    public class Fivem : ModuleBase<SocketCommandContext>
    {

        private readonly WebApiService _webApiService = new WebApiService();
        private readonly SaverService _saverService = new SaverService();
        private readonly SteamService _steamService = new SteamService();
        private static string joinServerUrl = "https://madsword.site/loginapi/joinserver.php";
        private static string closeServerUrl = "https://madsword.site/loginapi/joinserver.php";
        private static string serverIpFinderUrl = "https://madsword.site/launcherapi/launcher.php?veri=ip";
        private static string serverPortFinderUrl = "https://madsword.site/launcherapi/launcher.php?veri=port";
        [Command("steamid")]
        public async Task FivemHex([Remainder]string id)
        {
            var foundedSteamId = _steamService.ConvertSteamHexInt64(id.Replace("steam:", ""));
            if (foundedSteamId != 0)
            {
                id = foundedSteamId.ToString();
            }
            var convertedHex = "steam:";
            var steamprofile = _steamService.GetProfileSteamId(id);
            if (steamprofile.Profile == null) steamprofile = _steamService.GetProfileId(id);
            if (steamprofile.Profile != null)
            {
                convertedHex += _steamService.GetSteamHex(Convert.ToInt64(steamprofile.Profile.SteamId64));
                var eb = new EmbedBuilder();
                eb.WithDescription(convertedHex);
                var lastGame = "";
                if (steamprofile.Profile.MostPlayedGames != null)
                {
                    lastGame = "\n" + steamprofile.Profile.MostPlayedGames.MostPlayedGame[0].GameName.CdataSection + "\n" + steamprofile.Profile.MostPlayedGames.MostPlayedGame[0].HoursOnRecord + " Saat";
                }
                eb.WithAuthor(steamprofile.Profile.SteamId.CdataSection + lastGame);
                eb.WithThumbnailUrl(steamprofile.Profile.AvatarFull.CdataSection);
                await Context.Channel.SendMessageAsync("", false, eb.Build());
                return;
            }
            await ReplyAsync("Steam Profili bulunamadı!");
        }
        private string ExtractString(string s, string tag)
        {
            var startTag = "<" + tag + ">";
            int startIndex = s.IndexOf(startTag) + startTag.Length;
            int endIndex = s.IndexOf("</" + tag + ">", startIndex);
            return s.Substring(startIndex, endIndex - startIndex);
        }
        //[Command("giris")]
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
        //[Command("sunucu")]
        public async Task FivemFindAndJoin([Remainder]string servername)
        {
            var model = _saverService.Get(Context.Message.Author.Id);
            if (model.Hex == null)
            {
                await ReplyAsync("Mevcut kayıt bulunamadı, lütfen SteamID ile kayıt olun. " + Context.Message.Author.Mention);
                return;
            }

            var values = new Dictionary<string, string>();
            values.Add("steam_name", "Oyuncu");
            values.Add("steam_hex", model.Hex);
            values.Add("ip", "127.0.0.1");
            values.Add("server", servername);
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
        // [Command("cikis")]
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
        //[Command("ip")]
        public async Task FivemIp()
        {
            await FivemIp("vanaheimrp");
        }
        [Command("ipbul")]
        public async Task FivemIp(string servername)
        {
            var values = new Dictionary<string, string>();
            values.Add("server", servername);
            var ip = await _webApiService.GetPostAsync(serverIpFinderUrl, values, "127.0.0.1");
            values = new Dictionary<string, string>();
            values.Add("server", servername);
            var port = await _webApiService.GetPostAsync(serverPortFinderUrl, values, "127.0.0.1");
            if (ip != null && port != null)
            {
                try
                {
                    int.Parse(port);
                    await ReplyAsync(ip + ":" + port + " " + Context.Message.Author.Mention);
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
