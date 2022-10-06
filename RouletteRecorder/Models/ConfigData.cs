using Newtonsoft.Json;
using RouletteRecorder.Utils;
using System.Collections.ObjectModel;

namespace RouletteRecorder.Models
{
    public class ConfigData : BindingTarget
    {
        public ConfigData() : this(null, Monitors.MonitorType.Network)
        {
        }

        [JsonConstructor]
        private ConfigData(ObservableCollection<int> rouletteTypes, Monitors.MonitorType monitorType)
        {
            RouletteTypes = rouletteTypes ?? new ObservableCollection<int>();
            MonitorType = monitorType == Monitors.MonitorType.Network ? Monitors.MonitorType.Network : Monitors.MonitorType.Game;
        }
        [JsonProperty("roulette")]
        public ObservableCollection<int> RouletteTypes { get; set; }
        [JsonProperty("monitor")]
        public Monitors.MonitorType MonitorType { get; set; }
    }
}