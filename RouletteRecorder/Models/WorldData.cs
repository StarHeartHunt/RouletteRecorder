using Newtonsoft.Json;

namespace RouletteRecorder.Models
{
    public class WorldData
    {
        [JsonProperty("name")]
        public string LocalName;
        [JsonProperty("name_en")]
        public string EnglishName;
        [JsonProperty("dc")]
        public string LocalDataCenter;
        [JsonProperty("dc_en")]
        public string EnglishDataCenter;

        public override string ToString()
        {
            return LocalName;
        }
    }
}
