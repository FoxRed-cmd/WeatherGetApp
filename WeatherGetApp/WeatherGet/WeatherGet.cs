using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;


namespace WeatherGetApp
{
    internal class WeatherGet
    {
        public Dictionary<string, string>? Cities { get; private set; }

        private HttpClient? _client;
        private HtmlDocument? _pageSite;
        private HtmlNode? _node;
        private List<HtmlNode>? _nodes;

        private readonly string _address = "https://yandex.ru/pogoda/";

        private List<char>? _response;

        private WeatherInfo? _weatherInfo;

        public WeatherGet()
        {
            LoadCities();
        }

        public WeatherInfo? GetWeather() => GetWeather(string.Empty);

        public WeatherInfo? GetWeather(string cityName)
        {
            if (Cities.TryGetValue(cityName.ToLower(), out string? city) || cityName == string.Empty)
            {
                using (_client = new HttpClient())
                {
                    byte[] data = _client.GetByteArrayAsync($"{_address}{city}").Result;
                    string response = Encoding.UTF8.GetString(data);

                    _pageSite = new HtmlDocument();
                    _pageSite.LoadHtml(response);

                    _node = _pageSite.DocumentNode.QuerySelector(".b-page__container");
                    _nodes = _node.QuerySelectorAll("div").ToList();

                    RemoveEmptySelectors(_nodes);

                    _node = _nodes.FirstOrDefault(node => node.InnerText.Contains("Ощущается как") && node.FirstChild.Name == "a");

                    if (cityName == string.Empty)
                        cityName = _nodes[0].InnerText.Substring(_nodes[0].InnerText.LastIndexOf(';') + 1);

                    _nodes = _nodes.FindAll(node => node.FirstChild.Name == "i");

                    _response = new List<char>();
                    char[] chars = _node.InnerText.ToCharArray();

                    TextFormatting(chars);

                    string result = new(_response.ToArray());
                    result = result.Trim();
                    string[] strArr = result.Split(' ');

                    _weatherInfo = new WeatherInfo()
                    {
                        Temperature = strArr[0],
                    };

                    for (int i = 1; i < strArr.Length; i++)
                    {
                        if (strArr[i + 1] != "Ощущается")
                            _weatherInfo.Sky += $"{strArr[i]} ";
                        else
                        {
                            _weatherInfo.Sky += $"{strArr[i]}";
                            break;
                        }
                    }

                    _weatherInfo.FeelLikeTemperature = strArr[^1];
                    _weatherInfo.Wind = _nodes[0].InnerText;
                    _weatherInfo.Humidity = _nodes[1].InnerText;
                    _weatherInfo.Pressure = _nodes[2].InnerText;
                    _weatherInfo.City = cityName;


                    return _weatherInfo;
                }
            }
            else
            {
                return null;
            }
        }

        private void RemoveEmptySelectors(List<HtmlNode> nodes)
        {
            do
            {
                _node = nodes.Find(node => node.InnerText == string.Empty);

                if (_node != null)
                    nodes.Remove(_node);
                else
                    break;

            } while (true);
        }

        private void TextFormatting(char[] chars)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                if (char.IsUpper(chars[i]) || char.IsSymbol(chars[i]) || char.IsPunctuation(chars[i]))
                {
                    _response.Add(' ');
                    _response.Add(chars[i]);
                }
                else
                {
                    if (i + 1 != chars.Length)
                    {
                        if (char.IsNumber(chars[i]) && char.IsNumber(chars[i + 1]) == false)
                        {
                            _response.Add(chars[i]);
                        }
                        else
                        {
                            _response.Add(chars[i]);
                        }
                    }
                    else
                    {
                        if (char.IsNumber(chars[i]))
                        {
                            _response.Add(chars[i]);
                        }
                        else
                        {
                            _response.Add(chars[i]);
                        }
                    }
                }
            }
        }

        private void LoadCities()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            path = path.Remove(path.LastIndexOf("\\") + 1);
            path += "Resources\\cities.json";
            if (File.Exists(path))
            {
                using (Stream file = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader sr = new(file))
                    {
                        Cities = JsonSerializer.Deserialize<Dictionary<string, string>>(sr.ReadToEnd());
                    }
                }
            }
        }
    }
}
