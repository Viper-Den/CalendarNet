using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using AccuWeather;

namespace DestinyNet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Data _data;
        private Settings _settings;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _settings = LoadSettings();
            var weatherViewModel = new WeatherViewModel(new AccuWeatherManager(_settings.AccuWeather));
            _data = Load();
            MainWindow app = new MainWindow() { DataContext = new ManagerViewModel(_data, _settings, weatherViewModel) };
            app.Show();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            SaveSettings(_settings);
            Save(_data);
            base.OnExit(e);
        }

        public void Save(Data data)
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "//Data.json";
            var mapper = DestinyNetMapper.GetMapper();
            var s = JsonConvert.SerializeObject(mapper.Map<DataDTO>(data));
            File.WriteAllText(path, s);
        }
        public Data Load()
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "//Data.json";
            if (!File.Exists(path))
            {
                var d = new Data();
                File.WriteAllText(path, JsonConvert.SerializeObject(d, Formatting.Indented));
                return d;
            }
            else
            {
                var dataDTO = JsonConvert.DeserializeObject<DataDTO>(File.ReadAllText(path));
                var mapper = DestinyNetMapper.GetMapper();
                return mapper.Map<Data>(dataDTO);
            }
        }
        public void SaveSettings(Settings settings)
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "//Settings.json";
            var s = JsonConvert.SerializeObject(settings);
            File.WriteAllText(path, s);
        }
        public Settings LoadSettings()
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "//Settings.json";
            if (!File.Exists(path))
            {
                var settings = new Settings();
                File.WriteAllText(path, JsonConvert.SerializeObject(settings, Formatting.Indented));
                return settings;
            }
            else
            {
                return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path));
            }
        }
    }
}
