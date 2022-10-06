﻿#if DEBUG
using System.Reflection;
#endif
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Advanced_Combat_Tracker;

namespace RouletteRecorder
{
#if DEBUG
    public static class ReflectionExtensions
    {
        public static T GetFieldValue<T>(this object obj, string name)
        {
            // Set the flags so that private and public fields from instances will be found
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var field = obj.GetType().GetField(name, bindingFlags);
            return (T)field?.GetValue(obj);
        }
    }
#endif

    public class RecorderInit : IActPluginV1
    {
        private Label lblStatus;
        private Views.MainControl mainControl = null;

        public void DeInitPlugin()
        {
            if (lblStatus != null)
            {
                lblStatus.Text = "RouletteRecorder Unloaded.";
                lblStatus = null;
            }
            if (mainControl != null)
            {
                mainControl.DeInit();
            }
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
#if DEBUG
            var globalTabControl = ActGlobals.oFormActMain.GetFieldValue<TabControl>("tc1");
            var pluginsTabPage = ActGlobals.oFormActMain.GetFieldValue<TabPage>("tpPlugins");
            var pluginsTabControl = ActGlobals.oFormActMain.GetFieldValue<TabControl>("tcPlugins");

            globalTabControl.SelectedTab = pluginsTabPage;
            pluginsTabControl.SelectedTab = pluginScreenSpace;
#endif
            lblStatus = pluginStatusText;
            lblStatus.Text = "RouletteRecorder Started.";
            pluginScreenSpace.Text = "日随记录器";

            if (mainControl == null)
            {
                mainControl = new Views.MainControl();
                var host = new ElementHost()
                {
                    Dock = DockStyle.Fill,
                    Child = mainControl
                };

                pluginScreenSpace.Controls.Add(host);
            }

        }
    }
}
