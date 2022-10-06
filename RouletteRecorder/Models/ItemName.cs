using Newtonsoft.Json;

namespace RouletteRecorder.Models
{
    public class ItemName
    {
        [JsonProperty("chs")]
        public string Chinese = null;
        [JsonProperty("en")]
        public string English = null;
        [JsonProperty("ja")]
        public string Japanese = null;
        [JsonProperty("de")]
        public string German = null;
        [JsonProperty("fr")]
        public string French = null;

        public override string ToString()
        {
            return Chinese ?? English;
        }
    }
}
