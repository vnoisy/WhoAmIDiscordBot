using Bot.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bot.Service
{
    public class SteamService
    {
        private readonly WebApiService _webApiService = new WebApiService();
        public string GetSteamHex(Int64 id)
        {
            try
            {
                return Convert.ToInt64(id).ToString("x");
            }
            catch
            {
                return null;
            }
        }
        public Int64 ConvertSteamHexInt64(string hex)
        {
            try
            {
                return Convert.ToInt64(hex, 16);
            }
            catch
            {
                return 0;
            }
        }
        public SteamPlayerModel GetProfileSteamId(string steamid)
        {
            try
            {
                var doc = _webApiService.Get("https://steamcommunity.com/id/" + steamid + "?xml=1");
                XmlDocument docx = new XmlDocument();
                docx.LoadXml(doc);
                //var steamId = ExtractString(doc, "steamID64");
                return JsonConvert.DeserializeObject<SteamPlayerModel>(JsonConvert.SerializeXmlNode(docx)); ;
            }
            catch
            {
                return null;
            }
        }
        public SteamPlayerModel GetProfileId(string id)
        {
            try
            {
                string url = id;
                if (!url.Contains("https://"))
                {
                    url = "https://" + id;
                }
                if (!url.Contains("steamcommunity.com/profiles/") && !url.Contains("steamcommunity.com/id/"))
                {
                    url = "https://steamcommunity.com/profiles/" + id;
                }
                var doc = _webApiService.Get(url + "?xml=1");
                XmlDocument docx = new XmlDocument();
                docx.LoadXml(doc);
                //var steamId = ExtractString(doc, "steamID64");
                return JsonConvert.DeserializeObject<SteamPlayerModel>(JsonConvert.SerializeXmlNode(docx)); ;
            }
            catch
            {
                return null;
            }
        }
        private string ExtractString(string s, string tag)
        {
            var startTag = "<" + tag + ">";
            int startIndex = s.IndexOf(startTag) + startTag.Length;
            int endIndex = s.IndexOf("</" + tag + ">", startIndex);
            return s.Substring(startIndex, endIndex - startIndex);
        }
    }
}
