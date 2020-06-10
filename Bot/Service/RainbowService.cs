using Bot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Service
{
    public class RainbowService
    {
        private readonly WebApiService api = new WebApiService();
        public string RefreshProfile(string username)
        {
            try
            {
                var data = api.Get<SearchModel>("https://r6tab.com/api/search.php?platform=uplay&search=" + username);
                api.Read("https://r6tab.com/api/player.php?p_id=" + data.results.FirstOrDefault().p_id);
                return ("```Profile refreshed```");
            }
            catch
            {
                return ("Profile cannot refreshed.");
            }
        }
        public SearchModel GetPlayerProfile(string username)
        {
            try
            {
                return api.Get<SearchModel>("https://r6tab.com/api/search.php?platform=uplay&search=" + username);
            }
            catch
            {
                return new SearchModel();
            }
        }
    }
}
