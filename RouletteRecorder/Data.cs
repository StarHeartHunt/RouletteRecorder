using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using RouletteRecorder.Utils;
using Newtonsoft.Json;

namespace RouletteRecorder
{
    public partial class Data : StaticBindingTarget<Data>
    {
        public Dictionary<int, Models.InstanceData> Instances;
        public Dictionary<int, Models.ItemName> InstanceTypes;
        public Dictionary<int, Models.ItemName> Territories;
        public Dictionary<int, Models.ItemName> Patches;
        public Dictionary<int, Models.WorldData> Worlds;
        public Dictionary<int, Models.ItemName> Roulettes { get; set; }
        public Dictionary<int, Models.JobName> Jobs;

        private bool ReadData<T>(string path, string file, out T dict) where T : new()
        {
            try
            {
                string content = File.ReadAllText(Path.Combine(path, file));
                dict = JsonConvert.DeserializeObject<T>(content);
                return true;
            }
            catch
            {
                MessageBox.Show(string.Format("无法找到数据文件 {0} 或读取时发生错误", file), "RouletteRecorder");
                dict = new T();
                return false;
            }
        }

        public void Init()
        {
            var dataRoot = Path.Combine(Helper.GetPluginDir(), "data");

            ReadData(dataRoot, "instance.json", out Instances);
            ReadData(dataRoot, "type.json", out InstanceTypes);
            ReadData(dataRoot, "territory.json", out Territories);
            ReadData(dataRoot, "patch.json", out Patches);
            ReadData(dataRoot, "world.json", out Worlds);
            ReadData(dataRoot, "job.json", out Jobs);

            ReadData(dataRoot, "roulette.json", out Dictionary<int, Models.ItemName> roulettes);
            Roulettes = roulettes;

            IsLoaded = true;
            DataLoaded?.Invoke(this, EventArgs.Empty);
        }

        public bool IsLoaded = false;
        public event EventHandler DataLoaded;
    }
}
