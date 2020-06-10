using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Model
{
    public partial class SteamPlayerModel
    {
        [JsonProperty("?xml")]
        public Xml Xml { get; set; }

        [JsonProperty("profile")]
        public Profile Profile { get; set; }
    }

    public partial class Profile
    {
        [JsonProperty("steamID64")]
        public string SteamId64 { get; set; }

        [JsonProperty("steamID")]
        public AvatarFull SteamId { get; set; }

        [JsonProperty("onlineState")]
        public string OnlineState { get; set; }

        [JsonProperty("stateMessage")]
        public AvatarFull StateMessage { get; set; }

        [JsonProperty("privacyState")]
        public string PrivacyState { get; set; }

        [JsonProperty("visibilityState")]
        
        public long VisibilityState { get; set; }

        [JsonProperty("avatarIcon")]
        public AvatarFull AvatarIcon { get; set; }

        [JsonProperty("avatarMedium")]
        public AvatarFull AvatarMedium { get; set; }

        [JsonProperty("avatarFull")]
        public AvatarFull AvatarFull { get; set; }

        [JsonProperty("vacBanned")]
        
        public long VacBanned { get; set; }

        [JsonProperty("tradeBanState")]
        public string TradeBanState { get; set; }

        [JsonProperty("isLimitedAccount")]
        
        public long IsLimitedAccount { get; set; }

        [JsonProperty("customURL")]
        public AvatarFull CustomUrl { get; set; }

        [JsonProperty("memberSince")]
        public string MemberSince { get; set; }

        [JsonProperty("steamRating")]
        public string SteamRating { get; set; }

        [JsonProperty("hoursPlayed2Wk")]
        public string HoursPlayed2Wk { get; set; }

        [JsonProperty("headline")]
        public AvatarFull Headline { get; set; }

        [JsonProperty("location")]
        public AvatarFull Location { get; set; }

        [JsonProperty("realname")]
        public AvatarFull Realname { get; set; }

        [JsonProperty("summary")]
        public AvatarFull Summary { get; set; }

        [JsonProperty("mostPlayedGames")]
        public MostPlayedGames MostPlayedGames { get; set; }

        [JsonProperty("groups")]
        public Groups Groups { get; set; }
    }

    public partial class AvatarFull
    {
        [JsonProperty("#cdata-section")]
        public string CdataSection { get; set; }
    }

    public partial class Groups
    {
        [JsonProperty("group")]
        public List<Group> Group { get; set; }
    }

    public partial class Group
    {
        [JsonProperty("@isPrimary")]
        
        public long IsPrimary { get; set; }

        [JsonProperty("groupID64")]
        public string GroupId64 { get; set; }

        [JsonProperty("groupName", NullValueHandling = NullValueHandling.Ignore)]
        public AvatarFull GroupName { get; set; }

        [JsonProperty("groupURL", NullValueHandling = NullValueHandling.Ignore)]
        public AvatarFull GroupUrl { get; set; }

        [JsonProperty("headline", NullValueHandling = NullValueHandling.Ignore)]
        public AvatarFull Headline { get; set; }

        [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
        public AvatarFull Summary { get; set; }

        [JsonProperty("avatarIcon", NullValueHandling = NullValueHandling.Ignore)]
        public AvatarFull AvatarIcon { get; set; }

        [JsonProperty("avatarMedium", NullValueHandling = NullValueHandling.Ignore)]
        public AvatarFull AvatarMedium { get; set; }

        [JsonProperty("avatarFull", NullValueHandling = NullValueHandling.Ignore)]
        public AvatarFull AvatarFull { get; set; }

        [JsonProperty("memberCount", NullValueHandling = NullValueHandling.Ignore)]
        
        public long? MemberCount { get; set; }

        [JsonProperty("membersInChat", NullValueHandling = NullValueHandling.Ignore)]
        
        public long? MembersInChat { get; set; }

        [JsonProperty("membersInGame", NullValueHandling = NullValueHandling.Ignore)]
        
        public long? MembersInGame { get; set; }

        [JsonProperty("membersOnline", NullValueHandling = NullValueHandling.Ignore)]
        
        public long? MembersOnline { get; set; }
    }

    public partial class MostPlayedGames
    {
        [JsonProperty("mostPlayedGame")]
        public List<MostPlayedGame> MostPlayedGame { get; set; }
    }

    public partial class MostPlayedGame
    {
        [JsonProperty("gameName")]
        public AvatarFull GameName { get; set; }

        [JsonProperty("gameLink")]
        public AvatarFull GameLink { get; set; }

        [JsonProperty("gameIcon")]
        public AvatarFull GameIcon { get; set; }

        [JsonProperty("gameLogo")]
        public AvatarFull GameLogo { get; set; }

        [JsonProperty("gameLogoSmall")]
        public AvatarFull GameLogoSmall { get; set; }

        [JsonProperty("hoursPlayed")]
        public string HoursPlayed { get; set; }

        [JsonProperty("hoursOnRecord")]
        public string HoursOnRecord { get; set; }

        [JsonProperty("statsName")]
        public AvatarFull StatsName { get; set; }
    }

    public partial class Xml
    {
        [JsonProperty("@version")]
        public string Version { get; set; }

        [JsonProperty("@encoding")]
        public string Encoding { get; set; }

        [JsonProperty("@standalone")]
        public string Standalone { get; set; }
    }
}
