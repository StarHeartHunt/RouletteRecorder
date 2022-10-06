using Newtonsoft.Json;

namespace RouletteRecorder.Models
{
    public class InstanceData
    {
        [JsonProperty("name")]
        public ItemName Name;

        [JsonProperty("type")]
        public int Type;

        [JsonProperty("level")]
        public int Level;

        [JsonProperty("levelSync")]
        public int LevelSync;

        [JsonProperty("item")]
        public int ItemLevel;

        [JsonProperty("itemSync")]
        public int ItemLevelSync;

        [JsonProperty("memberType")]
        public int MemberType;
    }
}