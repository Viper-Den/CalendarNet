using System.Windows;
using AccuWeather;
using Destiny.Core;

namespace DestinyNet
{
    public class WindowSettings
    {
        private int _left;
        private int _top;
        public int Left { 
            get => _left; 
            set {
                if (value < 0)
                    _left = 0;
                else
                    _left = value;
            } 
        }
        public int Top { 
            get => _top; 
            set
            {
                if (value < 0)
                    _left = 0;
                else
                    _left = value;
            }
        }
        public WindowState WindowState { get; set; }
    }
    public class WeekSettings
    {
        private int _hourHeight;
        public bool HoursHided { get; set; }
        public bool IsEventsViewHide { get; set; }
        public int HourHeight { 
            get => _hourHeight; 
            set { 
                if(value < 10)
                    _hourHeight = 10;
                else
                    _hourHeight = value; 
            } 
        }  
    }

    public class Settings
    {
        public Settings()
        {
            WindowSettings = new WindowSettings();
            Palettes = new PaletteSettings();
            AccuWeather = new AccuWeatherManagerSettings();
            WeekSettings = new WeekSettings();
        }
        public WindowSettings WindowSettings { get; set; }
        public PaletteSettings Palettes { get; set; }
        public AccuWeatherManagerSettings AccuWeather { get; set; }
        public WeekSettings WeekSettings { get; set; }
    }
}
