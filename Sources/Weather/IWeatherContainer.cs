using System.Collections.ObjectModel;

namespace AccuWeather
{
    public interface IWeatherContainer
    {
        ObservableCollection<WeatherDay> Weather { get; }    
    }
}
