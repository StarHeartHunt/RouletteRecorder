using RouletteRecorder.Utils;
using System.Windows;

namespace RouletteRecorder.ViewModels
{

    internal class DungeonLoggerSetting : BindingTarget
    {
        public string Username { get; set; } = Config.Instance.DungeonLogger.Username;

        public string Password { get; set; } = Config.Instance.DungeonLogger.Password;
    }
}
