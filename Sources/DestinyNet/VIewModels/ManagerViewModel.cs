using System.ComponentModel;
using MonthEvent;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
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

            MonthViewCommand = new ActionCommand(ViewMonth);
            WeekViewCommand = new ActionCommand(ViewWeek);
            YearViewCommand = new ActionCommand(ViewYear);
            ToDoViewCommand = new ActionCommand(ViewToDo);
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

        #region Command
        public ICommand MonthViewCommand { get; }
        public ICommand WeekViewCommand { get; }
        public ICommand YearViewCommand { get; }
        public ICommand ToDoViewCommand { get; }
        private void ViewMonth(object o)
        {
            SelectiewModelEnum = ViewModelEnum.Month;
        }
        private void ViewWeek(object o)
        {
            SelectiewModelEnum = ViewModelEnum.Week;
        }
        private void ViewYear(object o)
        {
            SelectiewModelEnum = ViewModelEnum.Year;
        }
        private void ViewToDo(object o)
        {
            SelectiewModelEnum = ViewModelEnum.ToDo;
        }
        #endregion
    }
}
