using System;

namespace AccuWeather
{
    public class DayWeather
    {
        public DateTime Date { get; set; }
        public double TemperatureMinimum { get; set; }
        public double TemperatureMaximum { get; set; }
        public DayWeatherType Type { get; set; }
    }
}
