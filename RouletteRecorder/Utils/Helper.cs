using Advanced_Combat_Tracker;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RouletteRecorder.Utils
{
    internal class Helper
    {
        private static readonly int[] WaitTime = { 5, 5, 10, 15, 25 };
        public static IActPluginV1 Instance = null;
        public static async Task<IActPluginV1> GetFFXIVPlugin()
        {
            for (int i = 0; i < WaitTime.Length; ++i)
            {
                var plugin = ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(x => x.cbEnabled.Checked && x.lblPluginTitle.Text == "FFXIV_ACT_Plugin.dll");
                if (plugin != null)
                {
                    return plugin.pluginObj;
                }

                await Task.Delay(WaitTime[i] * 1000);
            }

            return null;
        }

        public static string GetDbPath()
        {
            return Path.Combine(GetPluginDir(), "数据.csv");
        }

        public static string GetConfigDir()
        {
            return Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config");
        }

        public static string GetPluginDir()
        {
            if (Instance != null)
            {
                foreach (ActPluginData plugin in ActGlobals.oFormActMain.ActPlugins)
                {
                    if (plugin.pluginObj == Instance)
                    {
                        return plugin.pluginFile.Directory.FullName;
                    }
                }
            }

            foreach (ActPluginData plugin in ActGlobals.oFormActMain.ActPlugins)
            {
                if (plugin.pluginFile.Name == "RouletteRecorder.dll")
                {
                    return plugin.pluginFile.Directory.FullName;
                }
            }

            string libPath = Assembly.GetExecutingAssembly().Location;
            if (libPath == "")
            {
                throw new Exception("Failed to locate the library.");
            }

            return Path.GetDirectoryName(libPath);
        }
    }
}
