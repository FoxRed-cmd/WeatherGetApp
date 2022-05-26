﻿using FontAwesome.WPF;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using WeatherGetApp.Pages;
using WeatherGetApp.Windows;

namespace WeatherGetApp
{
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr window, int index, int value);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr window, int index);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        public static void HideFromAltTab(IntPtr Handle)
        {
            SetWindowLong(Handle, GWL_EXSTYLE, GetWindowLong(Handle, GWL_EXSTYLE) | WS_EX_TOOLWINDOW);
        }

        private MainPage _mainPage;
        private SettingsWindow _settingsWindow;
        private MainViewModel _viewModel;
        private Timer _timer;
        private Action _getWeatherAction;
        private Action _updatePropAction;
        private Task _task;
        public MainWindow()
        {
            InitializeComponent();
            _mainPage = new MainPage();
            _timer = new Timer() { Interval = MinToMilliseconds(45), Enabled = true };

            mainFrame.Content = _mainPage;
            _viewModel = _mainPage.DataContext as MainViewModel;
            _getWeatherAction = _viewModel.GetWeather;
            _updatePropAction = _viewModel.UpdateWeatherInfo;

            DataContext = _viewModel;

            KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    _task = new Task(_getWeatherAction);
                    _task.Start();
                    Keyboard.ClearFocus();
                    _mainPage.LineOff();
                    while (!_task.IsCompleted) { };
                    _updatePropAction.Invoke();
                }
            };

            _timer.Tick += (s, e) =>
            {
                UpdateInfo();
            };
        }

        private int MinToMilliseconds(int min) => min * 60000;

        private void UpdateInfo()
        {
            _task = new Task(_getWeatherAction);
            _task.Start();
            while (!_task.IsCompleted) { };
            _updatePropAction.Invoke();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
            _mainPage.LineOff();
            DragMove();
            _viewModel.WriteConfig();
        }

        private void FontAwesome_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => System.Windows.Application.Current.Shutdown();

        private async void FontAwesome_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var icon = sender as FontAwesome.WPF.FontAwesome;
            if (icon.Spin == true)
                return;
            icon.BeginSpin();
            await Task.Delay(500);
            icon.StopSpin();
        }

        private void FontAwesome_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var icon = sender as FontAwesome.WPF.FontAwesome;
            icon.StopSpin();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HideFromAltTab(new WindowInteropHelper(this).Handle);

            _timer.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
            _viewModel.WriteConfig();
        }

        private void FontAwesome_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (!System.Windows.Application.Current.Windows.OfType<SettingsWindow>().Any())
            {
                _settingsWindow = new SettingsWindow() { DataContext = _viewModel };
                _settingsWindow.Show();
            }
        }
    }
}