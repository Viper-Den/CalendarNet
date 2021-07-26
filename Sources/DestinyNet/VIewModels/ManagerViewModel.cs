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
        public ManagerViewModel(Data data)
        {

            _data = data;
            DialogViewsManager = new DialogViewsManager();
            ToolPanel = new ToolPanelViewModel(_data, DialogViewsManager);
            _viewModelsDictionary = new Dictionary<ViewModelEnum, IViewModel>();
            _viewModelsDictionary.Add(ViewModelEnum.Month, new MonthViewModel(_data));
            _viewModelsDictionary.Add(ViewModelEnum.Week, new WeekViewModel(_data));
            _viewModelsDictionary.Add(ViewModelEnum.Year, new YearViewModel(_data));
            _viewModelsDictionary.Add(ViewModelEnum.ToDo, new ToDoViewModel(_data));
            _selectedViewModelEnum = ViewModelEnum.Month;
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
