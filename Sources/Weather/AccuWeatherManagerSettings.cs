using System;
using System.Collections.Generic;
using System.Text;

namespace AccuWeather
{
    public class AccuWeatherManagerSettings
    {
        public bool Enabled { get; set; }
        public string APIkey { get; set; } = "cGpLYlD4KlusGl3a55IPH7R27BXq5Mle";
        public string CityName { get; set; } = "";
        public string CityKey { get; set; } = "";
    }
}
