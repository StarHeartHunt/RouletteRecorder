using Advanced_Combat_Tracker;
using RouletteRecorder.Constant;
using RouletteRecorder.DAO;
using RouletteRecorder.Monitors;
using RouletteRecorder.Utils;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace RouletteRecorder.Views
{
    public partial class MainControl : UserControl
    {
        private IActPluginV1 ffxivPlugin = null;
        public MainControl()
        {
            Config.Load();
            InitializeComponent();
            Init();
        }

        private ViewModels.MainViewModel viewModel = null;
        private ViewModels.MainViewModel ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = (ViewModels.MainViewModel)DataContext;
                }

                return viewModel;
            }
        }

        private async void Init()
        {
            Utils.Log.Handler += Log;
            Data.Instance.PropertyChanged += Data_PropertyChanged;
            ViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "RouletteTypes")
                {
                    if (ViewModel.RouletteTypes.Count == 0) return;
                    var rouletteTypeNodes = ViewModel.RouletteTypes;

                    foreach (var rouletteTypeNode in rouletteTypeNodes)
                    {
                        rouletteTypeNode.IsChecked = Config.Instance.RouletteTypes.Contains(rouletteTypeNode.Id);
                        rouletteTypeNode.PropertyChanged += RouletteTypeNode_PropertyChanged;
                    }
                }
                else if (e.PropertyName == "SelectedMonitorIndex")
                {
                    Config.Instance.MonitorType = (MonitorType)ViewModel.SelectedMonitorIndex;
                }
            };

            Data.Instance.Init();
            Database.InitDatabase();

            var network = new NetworkMonitor();
            network.OnException += LogException;

            ffxivPlugin = await Helper.GetFFXIVPlugin();
            ParsePlugin.Init(ffxivPlugin, network);

            if (Config.Instance.Language == null)
            {
                Config.Instance.Language = ParsePlugin.Instance.GetLanguage();
            }

            if (Config.Instance.Region == null)
            {
                Config.Instance.Region = ParsePlugin.Instance.GetRegion();
            }

            ParsePlugin.Instance.Network = network;
            ParsePlugin.Instance.Start();
        }
        public void DeInit()
        {
            if (ParsePlugin.Instance != null)
            {
                ParsePlugin.Instance.Stop();
            }

            Utils.Log.Handler -= Log;
            Data.Instance.PropertyChanged -= Data_PropertyChanged;
            Database.Release();
        }

        private void LogException(Exception e)
        {
            try
            {
                Log(LogType.None, 'E', string.Format("[{0}]{1}\r\n{2}", e.GetType(), e.Message, e.StackTrace));
            }
            catch { }
        }

        private Models.ConfigLogger ConfigLogger => Config.Instance.Logger;
        private void Log(LogType type, char level, string message)
        {
            var vm = ViewModel;
            ++vm.LogAllCount;

            if (
                !ConfigLogger.Enabled
                || vm.LogPause
                || (vm.LogTypeFilter && type != vm.LogTypeFilterValue)
#if DEBUG
                || (level == 'D' && !ConfigLogger.Debug)
#else
                || (level == 'I' && !ConfigLogger.Debug)
#endif
            )
            {
                return;
            }

            string typeString = Enum.GetName(typeof(LogType), type);
            vm.Log = string.Format("[{0}][{1}][{2}] {3}\r\n", DateTime.Now, level, typeString, message) + vm.Log;
            ++vm.LogShowCount;
        }

        private void Data_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Roulettes")
            {
                ViewModel.RouletteTypes = ViewModels.RouletteTypeNodeList.Create(Data.Instance.Roulettes);
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Roulette.Init("test", "test2", true);
            Database.InsertRoulette(Roulette.Instance);
        }

        private void BDungeonSettingConfig_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new DungeonLoggerSetting
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            dialog.ShowDialog();
        }

        private void BLogClear_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Log = "";
        }

        private void BLogPause_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LogPause = !ViewModel.LogPause;
        }

        private void RouletteTypeNode_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsChecked")
            {
                var rouletteTypes = Config.Instance.RouletteTypes;
                var node = (ViewModels.RouletteTypeNode)sender;
                var index = rouletteTypes.IndexOf(node.Id);
                if (node.IsChecked)
                {
                    if (index == -1)
                    {
                        rouletteTypes.Add(((ViewModels.RouletteTypeNode)sender).Id);
                    }
                }
                else
                {
                    if (index != -1)
                    {
                        rouletteTypes.RemoveAt(index);
                    }
                }
            }
        }
    }
}
