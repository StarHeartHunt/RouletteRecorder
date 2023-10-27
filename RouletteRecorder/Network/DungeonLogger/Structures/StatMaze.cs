using Newtonsoft.Json;

namespace RouletteRecorder.Network.DungeonLogger.Structures
{
    public class StatMaze
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }
    }
}
