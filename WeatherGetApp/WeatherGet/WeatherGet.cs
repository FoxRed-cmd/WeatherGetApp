using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
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
                    List<HtmlNode>? nodes;
                    byte[] data = _client.GetByteArrayAsync($"{_address}{city}").Result;
                    string response = Encoding.UTF8.GetString(data);

                    _pageSite = new HtmlDocument();
                    _pageSite.LoadHtml(response);

                    try
                    {
                        _node = _pageSite.DocumentNode.QuerySelector(".b-page__container");
                        _nodes = _node.QuerySelectorAll("div").ToList();
                        RemoveEmptySelectors(_nodes);
                        _node = _nodes.FirstOrDefault(node => node.InnerText.Contains("Ощущается как") && node.FirstChild.Name == "a");
                    }
                    catch (Exception)
                    {
                        return new() { City = "Не удалось обновить информацию :(" };
                    }

                    if (cityName == string.Empty)
                        cityName = _nodes[0].InnerText.Substring(_nodes[0].InnerText.LastIndexOf(';') + 1);

                    nodes = _nodes.FindAll(node => node.FirstChild.Name == "i");

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
                    _weatherInfo.Wind = nodes[0].InnerText;
                    _weatherInfo.Humidity = nodes[1].InnerText;
                    _weatherInfo.Pressure = nodes[2].InnerText;
                    _weatherInfo.City = cityName;

                    nodes = _nodes.FindAll(node => node.InnerHtml.Contains("forecast-briefly__name") && node.InnerHtml.Contains("temp__value temp__value_with-unit")
                    && node.InnerHtml.Contains("forecast-briefly__condition") && node.ParentNode.Name == "a");

                    for (int i = 2; i < 12; i++)
                    {
                        _weatherInfo.DaysWeather.Add(AddDayWeatherTextFormatting(nodes[i].InnerText.ToCharArray()));
                    }

                    return _weatherInfo;
                }
            }
            else
            {
                return new() { City = "Не удалось обновить информацию :(" };
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

        private WeatherInfo.DaysWeatherInfo AddDayWeatherTextFormatting(char[] chars)
        {
            _response.Clear();
            bool isLetter = true;
            bool isNumber = false;
            bool isSign = false;
            for (int i = 0; i < chars.Length; i++)
            {
                if (char.IsLetter(chars[i]) && isLetter)
                {
                    _response.Add(chars[i]);
                    isNumber = false;
                    isSign = false;
                }
                else if ((char.IsNumber(chars[i]) || char.IsDigit(chars[i])) && isNumber)
                {
                    _response.Add(chars[i]);
                    isLetter = false;
                    isSign = false;
                }
                else if ((char.IsNumber(chars[i]) || char.IsDigit(chars[i])) && !isNumber)
                {
                    isNumber = true;
                    isLetter = false;
                    isSign = false;
                    _response.Add(' ');
                    _response.Add(chars[i]);
                }
                else if ((char.IsSymbol(chars[i]) || char.IsPunctuation(chars[i])) && isSign)
                {
                    _response.Add(chars[i]);
                    isLetter = false;
                    isNumber = false;
                }
                else if ((char.IsSymbol(chars[i]) || char.IsPunctuation(chars[i])) && !isSign)
                {
                    isSign = true;
                    isNumber = false;
                    isLetter = false;
                    _response.Add(' ');
                    _response.Add(chars[i]);
                }
                else if (char.IsLetter(chars[i]) && !isLetter)
                {
                    isLetter = true;
                    isNumber = false;
                    isSign = false;
                    _response.Add(' ');
                    _response.Add(chars[i]);
                }
                else
                {
                    _response.Add(chars[i]);
                }
            }

            string str = new string(_response.ToArray());
            string[] arr = str.Split(' ');

            WeatherInfo.DaysWeatherInfo daysWeatherInfo = new()
            { Day = arr[0], Date = $"{arr[1]} {arr[3]}", DayTemperature = "Днём " + arr[4] + arr[5], NightTemperature = "Ночью " + arr[6] + arr[7] };

            for (int i = 8; i < arr.Length; i++)
            {
                if (i != arr.Length - 1)
                {
                    daysWeatherInfo.Sky += $"{arr[i]} ";
                }
                else
                {
                    daysWeatherInfo.Sky += arr[i];
                }

            }

            return daysWeatherInfo;
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
            else
            {
                path = Assembly.GetExecutingAssembly().Location;
                path = path.Remove(path.LastIndexOf("\\") + 1);
                path += "cities.json";
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
}
