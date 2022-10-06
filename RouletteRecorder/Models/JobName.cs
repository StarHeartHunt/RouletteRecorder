using Newtonsoft.Json;

namespace RouletteRecorder.Models
{
    public class JobName
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
