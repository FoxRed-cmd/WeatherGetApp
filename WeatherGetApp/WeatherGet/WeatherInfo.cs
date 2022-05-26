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

        public WeatherInfo()
        {
            MeasureSymbol = "°C";
        }

        public override string ToString() => $"Город - {City}\nТепература - {Temperature}\nНебо - {Sky}\nОщущается как - {FeelLikeTemperature}\n" +
            $"Ветер - {Wind}\nВлажность - {Humidity}\nДавление - {Pressure}";
    }
}
