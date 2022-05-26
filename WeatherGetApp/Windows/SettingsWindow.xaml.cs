using System.Windows;

namespace WeatherGetApp.Windows
{
    public partial class SettingsWindow : Window
    {
        private MainViewModel _viewModel;
        private bool _isLoaded = false;
        public SettingsWindow()
        {
            InitializeComponent();
            checkDynamic.Checked += (s, e) =>
            {
                if (_isLoaded)
                {
                    _viewModel.DynamicBackground();
                }
            };
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_viewModel == null)
                _viewModel = DataContext as MainViewModel;
            _viewModel.ReadConfig();
            _isLoaded = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _viewModel.WriteConfig();
        }
    }
}
