using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DestinyNet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Data _data;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _data = new Data();
            Load();
            MainWindow app = new MainWindow() { DataContext = new ManagerViewModel(_data) };
            app.Show();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Save();
        }

        public void Save()
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "//Configuration.json";
            var s = JsonConvert.SerializeObject(_data);
            File.WriteAllText(path, s);
        }
        public void Load()
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "//Configuration.json";
            if (!File.Exists(path))
                File.WriteAllText(path, JsonConvert.SerializeObject(_data, Formatting.Indented));
            else
            {
                var s = File.ReadAllText(path);
                _data = JsonConvert.DeserializeObject<Data>(s);
            }
        }
    }
}
