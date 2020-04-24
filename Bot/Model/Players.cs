using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Model
{
    public class Players
    {
        public int id { get; set; }
        public ChampionModel Champion { get; set; }
        public SocketUser Player { get; set; }
    }
}
