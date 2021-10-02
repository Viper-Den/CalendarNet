using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using AccuWeather.DTO;

namespace AccuWeather
{
    public class AccuWeatherManager
    {
        private AccuWeatherManagerSettings _settings;
        private const int MAX_ICON_INDEX = 44;
        private static Dictionary<int, DayWeatherType> _dayWeatherTypeDictionary;
        // https://developer.accuweather.com/
        // https://habr.com/ru/post/544592/
        public AccuWeatherManager(AccuWeatherManagerSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }
        static AccuWeatherManager()
        {
            //https://developer.accuweather.com/weather-icons
            _dayWeatherTypeDictionary = new Dictionary<int, DayWeatherType>();

            _dayWeatherTypeDictionary.Add(1, DayWeatherType.Sunny_Clear);
            _dayWeatherTypeDictionary.Add(2, DayWeatherType.Sunny_Clear);

            _dayWeatherTypeDictionary.Add(3, DayWeatherType.Intermittent_clouds);
            _dayWeatherTypeDictionary.Add(4, DayWeatherType.Intermittent_clouds);
            _dayWeatherTypeDictionary.Add(5, DayWeatherType.Intermittent_clouds);
            
            _dayWeatherTypeDictionary.Add(6, DayWeatherType.Partially_cloudy);
            _dayWeatherTypeDictionary.Add(7, DayWeatherType.Cloudy);
            _dayWeatherTypeDictionary.Add(8, DayWeatherType.Overcast);
            _dayWeatherTypeDictionary.Add(11, DayWeatherType.Fog);

            for (var i = 12; i < 19; i++)
                _dayWeatherTypeDictionary.Add(i, DayWeatherType.Rain);

            _dayWeatherTypeDictionary[15] = DayWeatherType.ThunderStorm;

            _dayWeatherTypeDictionary.Add(19, DayWeatherType.Cloudy);
            _dayWeatherTypeDictionary.Add(20, DayWeatherType.Cloudy);
            _dayWeatherTypeDictionary.Add(21, DayWeatherType.Cloudy);

            _dayWeatherTypeDictionary.Add(22, DayWeatherType.Snow);
            _dayWeatherTypeDictionary.Add(23, DayWeatherType.Snow);
            _dayWeatherTypeDictionary.Add(24, DayWeatherType.Snow);


            _dayWeatherTypeDictionary.Add(25, DayWeatherType.Rain_Snow);
            _dayWeatherTypeDictionary.Add(26, DayWeatherType.Rain_Snow);
            _dayWeatherTypeDictionary.Add(27, DayWeatherType.Rain_Snow);

            for (var i = 1; i <= MAX_ICON_INDEX; i++){
                if (!_dayWeatherTypeDictionary.ContainsKey(i))
                    _dayWeatherTypeDictionary.Add(i, DayWeatherType.None);
            }
        }


        /// <summary>
          /// Метод реализует возможность получения списка городов.
          /// В качестве формального параметра принимается название города
          /// которое должно быть указано в классе MainMenu.
          /// </summary>
          /// <param name="formalCityName"></param>
        public string GetCityKey(string cityName)
        {
            var res = "";
            try
            {
                string request = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey={_settings.APIkey}&q={cityName}";
                WebClient webClient = new WebClient();
                string response = webClient.DownloadString(request);
                List<CityInfoDTO> cities = JsonConvert.DeserializeObject<List<CityInfoDTO>>(response);
                res = cities[0].Key;
            }
            catch (Exception ex)
            {
            }
            return res;
        }

        public List<DayWeather> GetFiveDaysWeather()
        {
            if (_settings.CityKey == "")
                _settings.CityKey = GetCityKey(_settings.CityName);
            var res = new List<DayWeather>();
            try
            {
                WebClient webClient = new WebClient();
                //                 http://dataservice.accuweather.com/forecasts/v1/daily/10day
                string request = $"http://dataservice.accuweather.com/forecasts/v1/daily/5day/{_settings.CityKey}?apikey={_settings.APIkey}&language=ru&metric=true";
                string response = webClient.DownloadString(request);
                var rootWeatherDTO = JsonConvert.DeserializeObject<RootWeatherDTO>(response);

                foreach (var d in rootWeatherDTO.DailyForecasts)
                {

                    var date = DateTime.Today;
                    if (d.Date != null)
                        date = (DateTime)d.Date;
                    res.Add(new DayWeather()
                    {
                        Date = date,
                        TemperatureMaximum = d.Temperature.Maximum.Value,
                        TemperatureMinimum = d.Temperature.Minimum.Value,
                        Type = _dayWeatherTypeDictionary[d.Day.Icon]
                    });
                } 
            }
            catch (Exception ex)
            {
            }
            return res;
        }
    }

}
