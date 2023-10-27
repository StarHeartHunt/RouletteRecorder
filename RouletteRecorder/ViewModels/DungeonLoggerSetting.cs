using RouletteRecorder.Utils;

namespace RouletteRecorder.ViewModels
{

    internal class DungeonLoggerSetting : BindingTarget
    {
        public string Password { get; set; } = Config.Instance.DungeonLogger.Password;
    }
}
