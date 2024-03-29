﻿using RouletteRecorder.Network.DungeonLogger;
using System;
using System.Windows;

namespace RouletteRecorder.Views
{
    /// <summary>
    /// Interaction logic for DungeonLoggerSetting.xaml
    /// </summary>
    public partial class DungeonLoggerSetting : Window
    {
        private ViewModels.DungeonLoggerSetting _viewModel = null;
        private ViewModels.DungeonLoggerSetting ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = (ViewModels.DungeonLoggerSetting)DataContext;
                }

                return _viewModel;
            }
        }
        public DungeonLoggerSetting()
        {
            InitializeComponent();
        }

        private void IPasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = iPasswordTextBox.Password;
        }

        private async void BDungeonSettingConfigSave_ClickAsync(object sender, RoutedEventArgs e)
        {
            Config.Instance.DungeonLogger.Password = ViewModel.Password;
            using (var client = new DungeonLoggerClient())
            {
                try
                {
                    var response = await client.PostLogin(Config.Instance.DungeonLogger.Password, Config.Instance.DungeonLogger.Username);
                    MessageBox.Show(response.Msg);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
