using System;
using System.Collections.Generic;
using System.Text;

namespace AccuWeather.DTO
{
    public class HeadlineDTO
    {
        public string EffectiveDate { get; set; }
        public int EffectiveEpochDate { get; set; }
        public int Severity { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public string EndDate { get; set; }
        public string EndEpochDate { get; set; }
        public string MobileLink { get; set; }
        public string Link { get; set; }
    }

    public class MinimumDTO
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class MaximumDTO
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class TemperatureDTO
    {
        public MinimumDTO Minimum { get; set; }
        public MaximumDTO Maximum { get; set; }
    }

    public class DayDTO
    {
        public int Icon { get; set; }
        public string IconPhrase { get; set; }
        public bool HasPrecipitation { get; set; }
        public string PrecipitationType { get; set; }
        public string PrecipitationIntensity { get; set; }
    }

    public class NightDTO
    {
        public int Icon { get; set; }
        public string IconPhrase { get; set; }
        public bool HasPrecipitation { get; set; }
        public string PrecipitationType { get; set; }
        public string PrecipitationIntensity { get; set; }
    }

    public class DailyForecastDTO
    {
        public DateTime? Date { get; set; }
        public int EpochDate { get; set; }
        public TemperatureDTO Temperature { get; set; }
        public DayDTO Day { get; set; }
        public NightDTO Night { get; set; }
        public List<string> Sources { get; set; }
        public string MobileLink { get; set; }
        public string Link { get; set; }
    }

    /// <summary>
    /// Класс реализует возможность хранения информации о погоде
    /// </summary>
    public class RootWeatherDTO
    {
        public HeadlineDTO Headline { get; set; }
        public List<DailyForecastDTO> DailyForecasts { get; set; }
    }
}

