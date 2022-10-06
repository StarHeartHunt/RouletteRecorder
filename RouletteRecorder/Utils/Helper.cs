using Advanced_Combat_Tracker;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RouletteRecorder.Utils
{
    public class Helper
    {
        public static IActPluginV1 GetFFXIVPlugin()
        {
            var plugin = ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(x => x.cbEnabled.Checked && x.lblPluginTitle.Text == "FFXIV_ACT_Plugin.dll");
            if (plugin != null)
            {
                return plugin.pluginObj;
            }
            else
            {
                throw new Exception("Cannot find FFXIV_ACT_Plugin, Please load it first!");
            }
        }

        public static Process GetFFXIVProcess()
        {
            var ffxivPlugin = (FFXIV_ACT_Plugin.FFXIV_ACT_Plugin)Helper.GetFFXIVPlugin();

            Process FFXIV = ffxivPlugin.DataRepository.GetCurrentFFXIVProcess() ?? Process.GetProcessesByName("ffxiv_dx11").FirstOrDefault();
            if (FFXIV != null)
            {
                return FFXIV;
            }
            else
            {
                throw new Exception("Could not find ffxiv_dx11.exe process. Make sure you are running the game and in DX11 mode.");
            }
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
