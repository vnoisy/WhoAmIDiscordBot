using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Model
{
    public class GameModel
    {
        public ulong ChannelId { get; set; }
        public ulong CreatorId { get; set; }
        public SocketUser User { get; set; }
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
