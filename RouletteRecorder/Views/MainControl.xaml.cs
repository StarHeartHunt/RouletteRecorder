using Advanced_Combat_Tracker;
using RouletteRecorder.Monitors;
using RouletteRecorder.Utils;
using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace RouletteRecorder.Views
{
    public partial class MainControl : UserControl
    {
        private IActPluginV1 ffxivPlugin = null;
        Injector injector;
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
        private void Init()
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
            ffxivPlugin = Helper.GetFFXIVPlugin();
            ParsePlugin.Init(ffxivPlugin, network);
            ParsePlugin.Instance.Network = network;
            ParsePlugin.Instance.Start();

            if (Config.Instance.MonitorType == MonitorType.Game)
            {
                injector = new Injector();
                injector.Init();
                injector.Start();
            }
        }
        public void DeInit()
        {
            Utils.Log.Handler -= Log;
            Data.Instance.PropertyChanged -= Data_PropertyChanged;
            Database.Release();
            ParsePlugin.Instance.Stop();
            if (injector != null)
            {
                injector.Stop();
            }
        }

        private void LogException(Exception e)
        {
            try
            {
                Log('E', string.Format("[{0}]{1}\r\n{2}", e.GetType(), e.Message, e.StackTrace));
            }
            catch { }
        }

        private void Log(char type, string message)
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                ViewModel.Log = string.Format("[{0}][{1}]{2}\r\n", DateTime.Now, type, message) + ViewModel.Log;
            });
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
            RouletteSingleton.Instance.Init("test", "test2", true);
            Database.InsertRoulette(RouletteSingleton.Instance);
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            Utils.Log.Info(viewModel.SelectedMonitorIndex.ToString());
            Utils.Log.Info(((int)Config.Instance.MonitorType).ToString());
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
