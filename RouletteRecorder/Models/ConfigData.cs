using FFXIV_ACT_Plugin.Common;
using Newtonsoft.Json;
using RouletteRecorder.Constant;
using RouletteRecorder.Utils;
using System.Collections.ObjectModel;

namespace RouletteRecorder.Models
{
    public class ConfigData : BindingTarget
    {
        public ConfigData() : this(null, null, null, null, null)
        {
        }

        [JsonConstructor]
        private ConfigData(
            Region? region,
            Language? language,
            ConfigLogger logger,
            ObservableCollection<int> rouletteTypes,
            ConfigDungeonLogger dungeonLogger)
        {
            Region = region;
            Language = language;
            RouletteTypes = rouletteTypes ?? new ObservableCollection<int>();
            MonitorType = Monitors.MonitorType.Network;
            Logger = logger ?? new ConfigLogger();
            DungeonLogger = dungeonLogger ?? new ConfigDungeonLogger();
        }

        [JsonProperty("region")]
        public Region? Region { get; set; }

        [JsonProperty("language")]
        public Language? Language { get; set; }

        [JsonProperty("roulette")]
        public ObservableCollection<int> RouletteTypes { get; set; }

        [JsonProperty("monitor")]
        public Monitors.MonitorType MonitorType { get; set; }

        [JsonProperty("logger")]
        public ConfigLogger Logger { get; set; }

        [JsonProperty("dungeonLogger")]
        public ConfigDungeonLogger DungeonLogger { get; set; }
    }

    public class ConfigLogger : BindingTarget
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
        [JsonProperty("debug")]
        public bool Debug { get; set; }
    }

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