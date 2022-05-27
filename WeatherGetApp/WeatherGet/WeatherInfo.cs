using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace WeatherGetApp
{
    internal class WeatherInfo
    {
        public string? City { get; set; }
        public string? Temperature { get; set; }
        public string? MeasureSymbol { get; set; }
        public string? Sky { get; set; }
        public string? FeelLikeTemperature { get; set; }
        public string? Wind { get; set; }
        public string? Humidity { get; set; }
        public string? Pressure { get; set; }
        public List<DaysWeatherInfo> DaysWeather { get; set; }

        public WeatherInfo()
        {
            MeasureSymbol = "°C";
            DaysWeather = new List<DaysWeatherInfo>();
        }

        public override string ToString() => $"Город - {City}\nТепература - {Temperature}\nНебо - {Sky}\nОщущается как - {FeelLikeTemperature}\n" +
            $"Ветер - {Wind}\nВлажность - {Humidity}\nДавление - {Pressure}";

        public class DaysWeatherInfo
        {
            public string? Day { get; set; }
            public string? Date { get; set; }
            public string? DayTemperature { get; set; }
            public string? NightTemperature { get; set; }
            public string? Sky { get; set; }
            public BitmapImage Icon { get; set; }
        }
    }
}
