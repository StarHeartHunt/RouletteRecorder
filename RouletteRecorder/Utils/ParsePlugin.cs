using Advanced_Combat_Tracker;
using FFXIV_ACT_Plugin.Common;

namespace RouletteRecorder.Utils
{
    internal class ParsePlugin
    {
        public static ParsePlugin Instance { get; private set; } = null;

        public static void Init(IActPluginV1 plugin, Monitors.NetworkMonitor network)
        {
            Instance = new ParsePlugin(plugin, network);
        }

        private readonly FFXIV_ACT_Plugin.FFXIV_ACT_Plugin _parsePlugin;

        public Monitors.NetworkMonitor Network { private get; set; }

        public ParsePlugin(IActPluginV1 plugin, Monitors.NetworkMonitor network)
        {
            _parsePlugin = (FFXIV_ACT_Plugin.FFXIV_ACT_Plugin)plugin;
            Network = network;
        }

        public void Start()
        {
            _parsePlugin.DataSubscription.NetworkReceived += HandleMessageReceived;
            _parsePlugin.DataSubscription.NetworkSent += HandleMessageSent;
        }

        public void Stop()
        {
            _parsePlugin.DataSubscription.NetworkReceived -= HandleMessageReceived;
            _parsePlugin.DataSubscription.NetworkSent -= HandleMessageSent;
        }

        public Language GetLanguage()
        {
            return _parsePlugin.DataRepository.GetSelectedLanguageID();
        }

        public uint GetServer()
        {
            var combatantList = _parsePlugin.DataRepository.GetCombatantList();
            if (combatantList == null || combatantList.Count == 0)
            {
                return 0;
            }

            return combatantList[0].CurrentWorldID;
        }

        public uint GetCurrentTerritoryID()
        {
            return _parsePlugin.DataRepository.GetCurrentTerritoryID();
        }

        private void HandleMessageSent(string connection, long epoch, byte[] message)
        {
            Network?.HandleMessageSent(connection, epoch, message);
        }

        private void HandleMessageReceived(string connection, long epoch, byte[] message)
        {
            Network?.HandleMessageReceived(connection, epoch, message);
        }
    }
}
