using System;
using System.Collections.Generic;

namespace AccuWeather.DTO
{
    public class RegionDTO
    {
        public string ID { get; set; }
        public string LocalizedName { get; set; }
        public string EnglishName { get; set; }
    }

    public class CountryDTO
    {
        public string ID { get; set; }
        public string LocalizedName { get; set; }
        public string EnglishName { get; set; }
    }

    public class AdministrativeAreaDTO
    {
        public string ID { get; set; }
        public string LocalizedName { get; set; }
        public string EnglishName { get; set; }
        public int Level { get; set; }
        public string LocalizedType { get; set; }
        public string EnglishType { get; set; }
        public string CountryID { get; set; }
    }

    public class TimeZoneDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double GmtOffset { get; set; }
        public bool IsDaylightSaving { get; set; }
        public object NextOffsetChange { get; set; }
    }

    public class MetricDTO
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class ImperialDTO
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class ElevationDTO
    {
        public MetricDTO Metric { get; set; }
        public ImperialDTO Imperial { get; set; }
    }

    public class GeoPositionDTO
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ElevationDTO Elevation { get; set; }
    }

    public class SupplementalAdminAreaDTO
    {
        public int Level { get; set; }
        public string LocalizedName { get; set; }
        public string EnglishName { get; set; }
    }
    public class CityInfoDTO
    {
        public int Version { get; set; }
        public string Key { get; set; }
        public string Type { get; set; }
        public int Rank { get; set; }
        public string LocalizedName { get; set; }
        public string EnglishName { get; set; }
        public string PrimaryPostalCode { get; set; }
        public RegionDTO Region { get; set; }
        public CountryDTO Country { get; set; }
        public AdministrativeAreaDTO AdministrativeArea { get; set; }
        public TimeZoneDTO TimeZone { get; set; }
        public GeoPositionDTO GeoPosition { get; set; }
        public bool IsAlias { get; set; }
        public List<SupplementalAdminAreaDTO> SupplementalAdminAreas { get; set; }
        public List<string> DataSets { get; set; }
    }

}
