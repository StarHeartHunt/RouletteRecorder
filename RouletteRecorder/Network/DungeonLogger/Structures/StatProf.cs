using Newtonsoft.Json;

namespace RouletteRecorder.Network.DungeonLogger.Structures
{
    public class StatProf
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("nameCn")]
        public string NameCn { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
