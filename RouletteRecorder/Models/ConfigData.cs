using Newtonsoft.Json;
using RouletteRecorder.Utils;
using System.Collections.ObjectModel;

namespace RouletteRecorder.Models
{
    public class ConfigData : BindingTarget
    {
        public ConfigData() : this(null, null)
        {
        }

        [JsonConstructor]
        private ConfigData(ObservableCollection<int> rouletteTypes, ConfigDungeonLogger dungeonLogger)
        {
            RouletteTypes = rouletteTypes ?? new ObservableCollection<int>();
            MonitorType = Monitors.MonitorType.Network;
            DungeonLogger = dungeonLogger ?? new ConfigDungeonLogger();
        }

        [JsonProperty("roulette")]
        public ObservableCollection<int> RouletteTypes { get; set; }

        [JsonProperty("monitor")]
        public Monitors.MonitorType MonitorType { get; set; }

        [JsonProperty("dungeonLogger")]
        public ConfigDungeonLogger DungeonLogger { get; set; }

        public class ConfigDungeonLogger : BindingTarget
        {
            [JsonProperty("enabled")]
            public bool Enabled { get; set; } = false;

            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }
        }
    }
}