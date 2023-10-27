using Newtonsoft.Json;

namespace RouletteRecorder.Network.DungeonLogger.Structures
{
    public class Response<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }
    }
}
