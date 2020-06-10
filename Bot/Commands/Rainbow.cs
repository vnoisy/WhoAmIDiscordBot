using Bot.Model;
using Bot.Service;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Commands
{
    public class Rainbow : ModuleBase<SocketCommandContext>
    {
        private readonly RainbowService r6 = new RainbowService();
        [Command("rank")]
        public async Task RankAsync([Remainder]string username)
        {
            Error();
            if (username == null)
            {
                await ReplyAsync("You need to write ur Username.");
                await ReplyAsync("!rank vNoisy.VAG");
                return;
            }
            var data = r6.GetPlayerProfile(username);

            if (data == null || data.results == null)
            {
                await ReplyAsync("User not found.");
                return;
            }
            var playerData = data.results.FirstOrDefault();
            var eb = new EmbedBuilder();
            eb.WithDescription("Current MMR: " + playerData.p_currentmmr + "\nLevel: " + playerData.p_level + "\nKDA: " + (double)playerData.kd / 100);
            eb.WithAuthor(playerData.p_name, "https://ubisoft-avatars.akamaized.net/" + playerData.p_id + "/default_146_146.png", "");
            eb.WithThumbnailUrl("https://r6tab.com/images/rankimg.php?rank=" + playerData.p_currentrank);
            await Context.Channel.SendMessageAsync("", false, eb.Build());
            return;
        }
        [Command("refresh")]
        public async Task RefreshAsync([Remainder]string username)
        {
            Error();
            if (username != null)
            {
                var error = r6.RefreshProfile(username);
                await ReplyAsync(error);
            }
        }
        private async void Error()
        {
            await ReplyAsync("RAINBOW Api kaynaklı bir problemden dolayı devre dışı!");
            return;
        }
    }
}
