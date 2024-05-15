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
            switch (Config.Instance.Language)
            {
                case FFXIV_ACT_Plugin.Common.Language.Chinese:
                    return Chinese;
                case FFXIV_ACT_Plugin.Common.Language.English:
                    return English;
                case FFXIV_ACT_Plugin.Common.Language.French:
                    return French;
                case FFXIV_ACT_Plugin.Common.Language.German:
                    return German;
                case FFXIV_ACT_Plugin.Common.Language.Japanese:
                    return Japanese;
                default:
                    return Chinese ?? English;
            }
        }
    }
}
