using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WeatherGetApp.HelperClasses;

namespace WeatherGetApp
{
    internal class MainViewModel : BaseViewModel
    {
        #region Weather

        private string _city;
        private string _temperature;
        private string _measureSymbol;
        private string _sky;
        private string _feel;
        private string _wind;
        private string _humidity;
        private string _pressure;
        private string _dateTimeInfo;
        private string _updateTimeInfo;
        private List<WeatherInfo.DaysWeatherInfo> _nextDaysWeather;

        private WeatherGet _weatherGet;
        private WeatherInfo? _weatherInfo;
        private Task _dateTimeUpdate;
        private BitmapImage _weatherIcon;

        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    NotifyPropertyChanged(nameof(City));
                }
            }
        }
        public string Temperature
        {
            get => _temperature;
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    NotifyPropertyChanged(nameof(Temperature));
                }
            }
        }
        public string MeasureSymbol
        {
            get => _measureSymbol;
            set
            {
                if (_measureSymbol != value)
                {
                    _measureSymbol = value;
                    NotifyPropertyChanged(nameof(MeasureSymbol));
                }
            }
        }
        public string Sky
        {
            get => _sky;
            set
            {
                if (_sky != value)
                {
                    _sky = value;
                    NotifyPropertyChanged(nameof(Sky));
                }
            }
        }
        public string Feel
        {
            get => _feel;
            set
            {
                if (_feel != value)
                {
                    _feel = value;
                    NotifyPropertyChanged(nameof(Feel));
                }
            }
        }
        public string Wind
        {
            get => _wind;
            set
            {
                if (_wind != value)
                {
                    _wind = value;
                    NotifyPropertyChanged(nameof(Wind));
                }
            }
        }
        public string Humidity
        {
            get => _humidity;
            set
            {
                if (_humidity != value)
                {
                    _humidity = value;
                    NotifyPropertyChanged(nameof(Humidity));
                }
            }
        }
        public string Pressure
        {
            get => _pressure;
            set
            {
                if (_pressure != value)
                {
                    _pressure = value;
                    NotifyPropertyChanged(nameof(Pressure));
                }
            }
        }
        public BitmapImage WeatherIcon
        {
            get => _weatherIcon;
            set
            {
                if (_weatherIcon != value)
                {
                    _weatherIcon = value;
                    NotifyPropertyChanged(nameof(WeatherIcon));
                }
            }
        }
        public string DateTimeInfo
        {
            get => _dateTimeInfo;
            set
            {
                if (_dateTimeInfo != value)
                {
                    _dateTimeInfo = value;
                    NotifyPropertyChanged(nameof(DateTimeInfo));
                }
            }
        }
        public string UpdateTimeInfo
        {
            get => _updateTimeInfo;
            set
            {
                if (_updateTimeInfo != value)
                {
                    _updateTimeInfo = value;
                    NotifyPropertyChanged(nameof(UpdateTimeInfo));
                }
            }
        }
        public List<WeatherInfo.DaysWeatherInfo> NextDaysWeather
        {
            get => _nextDaysWeather;
            set
            {
                if (_nextDaysWeather != value)
                {
                    _nextDaysWeather = value;
                    NotifyPropertyChanged(nameof(NextDaysWeather));
                }
            }
        }
        #endregion

        #region Settings
        private double _top;
        private double _left;
        private bool _isAutorun;
        private Color _color1;
        private Color _color2;
        private Color _textColor;
        private Brush _textColorBrush;
        private double _angle;
        private bool _isDynamic;
        private int _timeDelay;

        private bool _isSettingsChanged = false;

        private RegistryKey? _read;
        private RegistryKey _write;
        private Task _task;
        private ColorHSL _colorHSV;

        public double Top
        {
            get => _top;
            set
            {
                if (_top != value)
                {
                    _top = value;
                    _isSettingsChanged = true;
                    NotifyPropertyChanged(nameof(Top));
                }
            }
        }
        public double Left
        {
            get => _left;
            set
            {
                if (_left != value)
                {
                    _left = value;
                    _isSettingsChanged = true;
                    NotifyPropertyChanged(nameof(Left));
                }
            }
        }
        public bool IsAutorun
        {
            get => _isAutorun;
            set 
            {
                if (_isAutorun != value)
                {
                    _isAutorun = value;
                    _isSettingsChanged = true;
                    NotifyPropertyChanged(nameof(IsAutorun));
                }
            }
        }
        public Color Color1
        {
            get => _color1;
            set
            {
                if (_color1 != value)
                {
                    _color1 = value;
                    _isSettingsChanged = true;
                    NotifyPropertyChanged(nameof(Color1));
                }
            }
        }
        public Color Color2
        {
            get => _color2;
            set
            {
                if (_color2 != value)
                {
                    _color2 = value;
                    _isSettingsChanged = true;
                    NotifyPropertyChanged(nameof(Color2));
                }
            }
        }
        public Color TextColor
        {
            get => _textColor;
            set 
            {
                if (_textColor != value)
                {
                    _textColor = value;
                    _isSettingsChanged = true;
                    NotifyPropertyChanged(nameof(TextColor));
                    TextColorBrush = new SolidColorBrush(_textColor);
                }
            }
        }
        public Brush TextColorBrush
        {
            get => _textColorBrush;
            set 
            {
                if (_textColorBrush != value)
                {
                    _textColorBrush = value;
                    _isSettingsChanged = true;
                    NotifyPropertyChanged(nameof(TextColorBrush));
                }
            }
        }
        public double Angle
        {
            get => _angle;
            set
            {
                if (_angle != value)
                {
                    _angle = value;
                    _isSettingsChanged = true;
                    NotifyPropertyChanged(nameof(Angle));
                }
            }
        }
        public bool IsDynamic
        {
            get => _isDynamic;
            set
            {
                if (_isDynamic != value)
                {
                    _isDynamic = value;
                    _isSettingsChanged = true;
                    NotifyPropertyChanged(nameof(IsDynamic));
                }
            }
        }
        public int TimeDelay
        {
            get => _timeDelay;
            set
            {
                if (_timeDelay != value)
                {
                    _timeDelay = value;
                    _isSettingsChanged = true;
                    NotifyPropertyChanged(nameof(TimeDelay));
                }
            }
        }
        #endregion

        

        public MainViewModel()
        {
            _weatherGet = new WeatherGet();
            City = "Поиск";
            Top = 50;
            Left = 50;
            Color1 = Color.FromArgb(255, 140, 162, 193);
            Color2 = Color.FromArgb(255, 51, 120, 212);
            TextColor = Color.FromArgb(255, 255, 255, 255);
            Angle = 45;
            IsDynamic = false;
            TimeDelay = 50;

            ReadConfig();
            UpdateDateTime();

            GetWeather();
            UpdateWeatherInfo();


            if (IsDynamic)
                DynamicBackground();

        }

        public void UpdateWeatherInfo()
        {
            if (_weatherInfo != null)
            {
                string temp = City;

                UpdateTimeInfo = $"Обновлено в {DateTime.Now:HH:mm}";

                City = _weatherInfo.City;
                Temperature = _weatherInfo.Temperature;
                MeasureSymbol = _weatherInfo.MeasureSymbol;
                Sky = _weatherInfo.Sky;
                Feel = "Ощущается как " + _weatherInfo.FeelLikeTemperature + _weatherInfo.MeasureSymbol;
                Wind = _weatherInfo.Wind;
                Humidity = _weatherInfo.Humidity;
                Pressure = _weatherInfo.Pressure;
                

                switch (Sky)
                {
                    case "Ясно":
                    case "Малооблачно":
                        WeatherIcon = new BitmapImage(new Uri("/Resources/SunStatus.png", UriKind.Relative));
                        break;
                    case "Пасмурно":
                        WeatherIcon = new BitmapImage(new Uri("/Resources/CloudStatus.png", UriKind.Relative));
                        break;
                    case "Облачно с прояснениями":
                        WeatherIcon = new BitmapImage(new Uri("/Resources/CloudStatus2.png", UriKind.Relative));
                        break;
                    case "Дождь":
                    case "Небольшой дождь":
                    case "Ливни":
                    case "Ливень":
                        WeatherIcon = new BitmapImage(new Uri("/Resources/RainStatus.png", UriKind.Relative));
                        break;
                    default:
                        break;
                }

                foreach (var day in _weatherInfo.DaysWeather)
                {
                    SetWeatherIcons(day);
                }

                NextDaysWeather = _weatherInfo.DaysWeather;

                if (temp == "Поиск")
                    WriteConfig();
            }
        }
        public void GetWeather()
        {
            if (City == "Поиск" || City == string.Empty || City == null)
                _weatherInfo = _weatherGet.GetWeather();
            else
                _weatherInfo = _weatherGet.GetWeather(City);
        }
        public void ReadConfig()
        {
            using (_read = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"))
            {
                if (_read == null)
                    return;
                if (_read.GetValue("WeatherGetApp") != null)
                    _isAutorun = true;
            }

            using (_read = Registry.CurrentUser.OpenSubKey("SOFTWARE\\WeatherGetApp"))
            {
                if (_read == null)
                {
                    using (_write = Registry.CurrentUser.CreateSubKey("SOFTWARE\\WeatherGetApp"))
                    {
                        _write.SetValue(nameof(Color1), $"{Color1.A},{Color1.R},{Color1.G},{Color1.B}", RegistryValueKind.String);
                        _write.SetValue(nameof(Color2), $"{Color2.A},{Color2.R},{Color2.G},{Color2.B}", RegistryValueKind.String);
                        _write.SetValue(nameof(TextColor), $"{TextColor.A},{TextColor.R},{TextColor.G},{TextColor.B}", RegistryValueKind.String);
                        _write.SetValue(nameof(Angle), Angle.ToString(), RegistryValueKind.String);
                        _write.SetValue(nameof(IsDynamic), IsDynamic, RegistryValueKind.String);
                        _write.SetValue(nameof(TimeDelay), TimeDelay.ToString(), RegistryValueKind.String);
                        _write.SetValue(nameof(Top), Top.ToString(), RegistryValueKind.String);
                        _write.SetValue(nameof(Left), Left.ToString(), RegistryValueKind.String);
                        _write.SetValue(nameof(City), City, RegistryValueKind.String);
                    }
                }
                else
                {
                    string[] argb = _read.GetValue(nameof(Color1)).ToString().Split(',');
                    Color1 = Color.FromArgb(byte.Parse(argb[0]), byte.Parse(argb[1]), byte.Parse(argb[2]), byte.Parse(argb[3]));
                    argb = _read.GetValue(nameof(Color2)).ToString().Split(',');
                    Color2 = Color.FromArgb(byte.Parse(argb[0]), byte.Parse(argb[1]), byte.Parse(argb[2]), byte.Parse(argb[3]));
                    argb = _read.GetValue(nameof(TextColor)).ToString().Split(',');
                    TextColor = Color.FromArgb(byte.Parse(argb[0]), byte.Parse(argb[1]), byte.Parse(argb[2]), byte.Parse(argb[3]));
                    Angle = double.Parse(_read.GetValue(nameof(Angle)).ToString());
                    IsDynamic = bool.Parse(_read.GetValue(nameof(IsDynamic)).ToString());
                    TimeDelay = int.Parse(_read.GetValue(nameof(TimeDelay)).ToString());
                    Top = double.Parse(_read.GetValue(nameof(Top)).ToString());
                    Left = double.Parse(_read.GetValue(nameof(Left)).ToString());
                    City = _read.GetValue(nameof(City)).ToString();
                }
            }

        }
        public void WriteConfig()
        {
            if (_isSettingsChanged == true)
            {
                if (_isAutorun == true)
                {
                    using (_write = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                    {
                        if (_write.GetValue("WeatherGetApp") == null)
                        {
                            string appPath = Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe");
                            _write.SetValue("WeatherGetApp", appPath);
                        }
                    }
                }
                else
                {
                    using (_write = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                    {
                        if (_write.GetValue("WeatherGetApp") != null)
                            _write.DeleteValue("WeatherGetApp");
                    }
                }

                using (_write = Registry.CurrentUser.CreateSubKey("SOFTWARE\\WeatherGetApp"))
                {
                    _write.SetValue(nameof(Color1), $"{Color1.A},{Color1.R},{Color1.G},{Color1.B}", RegistryValueKind.String);
                    _write.SetValue(nameof(Color2), $"{Color2.A},{Color2.R},{Color2.G},{Color2.B}", RegistryValueKind.String);
                    _write.SetValue(nameof(TextColor), $"{TextColor.A},{TextColor.R},{TextColor.G},{TextColor.B}", RegistryValueKind.String);
                    _write.SetValue(nameof(Angle), Angle.ToString(), RegistryValueKind.String);
                    _write.SetValue(nameof(IsDynamic), IsDynamic, RegistryValueKind.String);
                    _write.SetValue(nameof(TimeDelay), TimeDelay.ToString(), RegistryValueKind.String);
                    _write.SetValue(nameof(Top), Top.ToString(), RegistryValueKind.String);
                    _write.SetValue(nameof(Left), Left.ToString(), RegistryValueKind.String);
                    if (City != null)
                        _write.SetValue(nameof(City), City, RegistryValueKind.String);
                }

                _isSettingsChanged = false;
            }
        }
        public void DynamicBackground()
        {
            _task = new Task( async () => 
            {
                void DynamicColor1()
                {
                    _colorHSV = ColorHSL.ColorToHSL(Color1);
                    _colorHSV.Hue += 1;
                    if (_colorHSV.Hue >= 360) _colorHSV.Hue = 0;
                    Color1 = ColorHSL.HSLToRGB(_colorHSV, Color1.A);
                }
                void DynamicColor2()
                {
                    _colorHSV = ColorHSL.ColorToHSL(Color2);
                    _colorHSV.Hue++;
                    if (_colorHSV.Hue >= 360) _colorHSV.Hue = 0;
                    Color2 = ColorHSL.HSLToRGB(_colorHSV, Color2.A);
                }

                while (IsDynamic)
                {
                    if (Angle < 360)
                    {
                        ++Angle;
                    }
                    else
                    {
                        Angle = 0;
                        ++Angle;
                    }
                    DynamicColor1();
                    DynamicColor2();

                    await Task.Delay(TimeDelay);
                }
            });
            _task.Start();
        }
        private void UpdateDateTime()
        {
            _dateTimeUpdate = new Task( async () => 
            {
                while (true)
                {
                    DateTimeInfo = DateTime.Now.ToString("dddd dd-MM-yyyy HH:mm").ToUpper();
                    await Task.Delay(10000);
                }
                
            });
            _dateTimeUpdate.Start();
        }
        private void SetWeatherIcons(WeatherInfo.DaysWeatherInfo daysWeatherInfo)
        {
            switch (daysWeatherInfo.Sky.ToLower())
            {
                case "ясно":
                case "малооблачно":
                    daysWeatherInfo.Icon = new BitmapImage(new Uri("/Resources/SunStatus.png", UriKind.Relative));
                    break;
                case "пасмурно":
                    daysWeatherInfo.Icon = new BitmapImage(new Uri("/Resources/CloudStatus.png", UriKind.Relative));
                    break;
                case "облачно с прояснениями":
                    daysWeatherInfo.Icon = new BitmapImage(new Uri("/Resources/CloudStatus2.png", UriKind.Relative));
                    break;
                case "дождь":
                case "небольшой дождь":
                case "ливни":
                case "ливень":
                    daysWeatherInfo.Icon = new BitmapImage(new Uri("/Resources/RainStatus.png", UriKind.Relative));
                    break;
                default:
                    break;
            }
        }
    }
}