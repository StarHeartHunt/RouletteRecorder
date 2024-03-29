﻿using System;
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
                MessageBox.Show($"无法找到数据文件 {file} 或读取时发生错误", "RouletteRecorder");
                dict = new T();
                return false;
            }
        }

        public void Init()
        {
            var dataRoot = Path.Combine(Helper.GetPluginDir(), "data");

            ReadData(dataRoot, "instance.json", out Instances);
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
