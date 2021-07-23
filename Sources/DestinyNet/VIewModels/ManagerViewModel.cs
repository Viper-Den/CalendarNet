using System.ComponentModel;
using MonthEvent;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DestinyNet
{
    public class ManagerViewModel :  INotifyPropertyChanged
    {
        private readonly Data _data;
        private ViewModelEnum _selectedViewModelEnum;
        private Dictionary<ViewModelEnum, IViewModel> _viewModelsDictionary;
        public ManagerViewModel()
        {

            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "//Configuration.json";
            _data = new Data();
            if (!File.Exists(path))
                File.WriteAllText(path, JsonConvert.SerializeObject(_data, Formatting.Indented));
            else
            {
                var s = File.ReadAllText(path);
                _data = JsonConvert.DeserializeObject<Data>(s); 
            }

            DialogViewsManager = new DialogViewsManager();
            ToolPanel = new ToolPanelViewModel(_data, DialogViewsManager);
            ToolPanel.UpdateData += Save;
            _viewModelsDictionary = new Dictionary<ViewModelEnum, IViewModel>();
            _viewModelsDictionary.Add(ViewModelEnum.Month, new MonthViewModel(_data));
            _viewModelsDictionary.Add(ViewModelEnum.Week, new WeekViewModel(_data));
            _viewModelsDictionary.Add(ViewModelEnum.Year, new YearViewModel(_data));
            _viewModelsDictionary.Add(ViewModelEnum.ToDo, new ToDoViewModel(_data));
            _selectedViewModelEnum = ViewModelEnum.Month;
        }

        ~ ManagerViewModel()
        {
        }

        public void Save()
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "//Configuration.json";
            var s = JsonConvert.SerializeObject(_data);
            File.WriteAllText(path, s);
        }

        public IViewModel SelectedViewModel {  get { return _viewModelsDictionary[_selectedViewModelEnum]; } }
        public IViewModel ToolPanel  { get; }
        public IDialogViewsManager DialogViewsManager { get; }

        public ViewModelEnum SelectiewModelEnum
        {
            get { return (_selectedViewModelEnum); }
            set
            {
                _selectedViewModelEnum = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }


    public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
