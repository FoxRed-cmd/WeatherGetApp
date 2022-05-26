using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace WeatherGetApp.Pages
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            searchField.GotFocus += (s, e) => 
            {
                if (searchField.Text == "Введите город")
                    searchField.Text = string.Empty;
            };
            searchField.PreviewMouseLeftButtonUp += (s, e) => LineOn();
        }

        public async void LineOn()
        {
            txtLine.X1 = 150;
            txtLine.X2 = 150;
            txtLine.Visibility = Visibility.Visible;
            for (int i = 150, j = 150; i >= 10; i -= 10, j += 10)
            {
                txtLine.X1 = i;
                txtLine.X2 = j;
                await Task.Delay(10);
            }
        }
        public async void LineOff()
        {
            if (txtLine.Visibility == Visibility.Visible)
            {
                for (int i = (int)txtLine.X1, j = (int)txtLine.X2; i <= 150; i += 10, j -= 10)
                {
                    txtLine.X1 = i;
                    txtLine.X2 = j;
                    await Task.Delay(10);
                }
                txtLine.Visibility = Visibility.Hidden;
            }
        }
    }
}
