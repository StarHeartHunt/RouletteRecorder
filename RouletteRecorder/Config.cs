using Newtonsoft.Json;
using RouletteRecorder.Utils;
using System.Collections.Generic;
using System.IO;
using ThomasJaworski.ComponentModel;

namespace RouletteRecorder
{
    public sealed class Config : BindingTarget
    {
        private static string configPath = Path.Combine(Helper.GetConfigDir(), "RouletteRecorder.config");
        public static Dictionary<Monitors.MonitorType, string> MonitorTypes = new Dictionary<Monitors.MonitorType, string>()
        {
            { Monitors.MonitorType.Network, "网络解析" },
            { Monitors.MonitorType.Game,"内存解析" },
        };
        public static Models.ConfigData Instance { get; private set; } = new Models.ConfigData();
        public static void Load()
        {
            if (!File.Exists(configPath))
            {
                Save();
            }
            try
            {
                string content = File.ReadAllText(configPath);
                Instance = JsonConvert.DeserializeObject<Models.ConfigData>(content);
            }
            catch { }

            var listener = ChangeListener.Create(Instance);
            listener.PropertyChanged += (_, e) => Save();
            listener.CollectionChanged += (_, e) => Save();
        }

        public static void Save()
        {
            File.WriteAllText(configPath, JsonConvert.SerializeObject(Instance, Formatting.Indented));
        }

    }
}
