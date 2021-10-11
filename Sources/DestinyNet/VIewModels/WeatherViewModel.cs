using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AccuWeather;
using MonthEvent;

namespace DestinyNet
{
    public class ImageInfo
    {
        public string Path { get; set; }
        public string ToolTip { get; set; }
    }
    public class DayWeatherCollection : IDayWeather
    {
        private DayWeather _dayWeather;
        private static Dictionary<DayWeatherType, ImageInfo> _dayWeatherPathDictionary;

        static DayWeatherCollection()
        {
            _dayWeatherPathDictionary = new Dictionary<DayWeatherType, ImageInfo>();
            _dayWeatherPathDictionary.Add(DayWeatherType.None, new ImageInfo() { Path = "None.png", ToolTip= "None" });
            _dayWeatherPathDictionary.Add(DayWeatherType.Sunny_Clear, new ImageInfo() { Path = "SunnyClear.png", ToolTip = "Sunny" });
            _dayWeatherPathDictionary.Add(DayWeatherType.Intermittent_clouds, new ImageInfo() { Path = "IntermittentClouds.png", ToolTip = "IntermittentClouds" });
            _dayWeatherPathDictionary.Add(DayWeatherType.Partially_cloudy, new ImageInfo() { Path = "PartiallyCloudy.png", ToolTip = "Partially cloudy"});
            _dayWeatherPathDictionary.Add(DayWeatherType.Cloudy, new ImageInfo() { Path = "Cloudy.png", ToolTip = "Cloudy" });
            _dayWeatherPathDictionary.Add(DayWeatherType.Overcast, new ImageInfo() { Path = "Overcast.png", ToolTip = "Overcast" });
            _dayWeatherPathDictionary.Add(DayWeatherType.Fog, new ImageInfo() { Path = "Fog.png", ToolTip = "Fog" });
            _dayWeatherPathDictionary.Add(DayWeatherType.Rain, new ImageInfo() { Path = "Rain.png", ToolTip = "Rain" });
            _dayWeatherPathDictionary.Add(DayWeatherType.ThunderStorm, new ImageInfo() { Path = "ThunderStorm.png", ToolTip = "Thunder Storm" });
            _dayWeatherPathDictionary.Add(DayWeatherType.Snow, new ImageInfo() { Path = "Snow.png", ToolTip = "Snow" });
            _dayWeatherPathDictionary.Add(DayWeatherType.Sleet, new ImageInfo() { Path = "Sleet.png", ToolTip = "Sleet" });
            _dayWeatherPathDictionary.Add(DayWeatherType.Rain_Snow, new ImageInfo() { Path = "RainAndSnow.png", ToolTip = "Rain and snow" });
        }
        public DayWeatherCollection(DayWeather dayWeather)
        {
            _dayWeather = dayWeather ?? throw new ArgumentNullException(nameof(dayWeather)); 
        }
        public bool IsDate(DateTime date)
        {
            return date.Date == _dayWeather.Date.Date;
        }
        public string Temperature {
            
            get { return $"{_dayWeather.TemperatureMaximum} C° / {_dayWeather.TemperatureMinimum} C°"; }
        }
        public ImageSource Image { 
            get { return new BitmapImage(new Uri("pack://application:,,,/;component/Images/Weather/" + _dayWeatherPathDictionary[_dayWeather.Type].Path, UriKind.Absolute)); }
        }
        public string ToolTip { get => _dayWeatherPathDictionary[_dayWeather.Type].ToolTip; }

    }
    public class WeatherViewModel
    {
        private AccuWeatherManager _accuWeatherManager;
        public WeatherViewModel(AccuWeatherManager accuWeatherManager)
        {
            _accuWeatherManager = accuWeatherManager ?? throw new ArgumentNullException(nameof(accuWeatherManager));
            
            DayWeatherCollection = new Dictionary<DateTime, IDayWeather>();
            var list = _accuWeatherManager.GetFiveDaysWeather();


            foreach(var d in list)
                DayWeatherCollection.Add(d.Date.Date, new DayWeatherCollection(d));
        }
        public Dictionary<DateTime, IDayWeather> DayWeatherCollection { get; }
    }
}
