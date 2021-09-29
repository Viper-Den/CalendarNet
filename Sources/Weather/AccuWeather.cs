using System;

namespace AccuWeather
{
    public class AccuWeather
    {
        private const string _key = "cGpLYlD4KlusGl3a55IPH7R27BXq5Mle";
        private string _keyCity;
        private IWeatherContainer _weatherContainer;
        // https://developer.accuweather.com/
        // https://habr.com/ru/post/544592/
        public AccuWeather(IWeatherContainer weatherContainer)
        {
            GetCityKey();
        }


        private void GetCityKey()
        {

        }
    }
}
