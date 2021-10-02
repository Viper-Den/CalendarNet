using AccuWeather;
using Destiny.Core;
using System;

namespace DestinyNet
{
    public class WeatherSettingsViewModel : BaseViewModel
    {
        private AccuWeatherManagerSettings _settings;
        public WeatherSettingsViewModel(AccuWeatherManagerSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }
        public bool Enabled { 
            get => _settings.Enabled; 
            set {
                _settings.Enabled = value;
                OnPropertyChanged(Enabled);
            } 
        }
        public string APIkey {
            get => _settings.APIkey; 
            set {
                _settings.APIkey = value;
                OnPropertyChanged(APIkey);
            }
        }
        public string CityName { 
            get => _settings.CityName;
            set {
                _settings.CityName = value;
                OnPropertyChanged(CityName);
            } 
        }
        public string CityKey { 
            get => _settings.CityKey;
            set {
                _settings.CityKey = value;
                OnPropertyChanged(CityKey);
            } 
        }
    }
}
