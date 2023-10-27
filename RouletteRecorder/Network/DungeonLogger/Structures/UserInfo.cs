using Newtonsoft.Json;
using System;

namespace RouletteRecorder.Network.DungeonLogger.Structures
{
    public class UserInfo
    {
        [JsonProperty("admin")]
        public Boolean Admin { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("target")]
        public int Target { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
