using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Input;
using Destiny.Core;

namespace DestinyNet
{
    public class ManagerViewModel :  BaseViewModel
    {
        private readonly Data _data;
        private ViewModelEnum _selectedViewModelEnum;
        private BaseViewModel _toolPanel;
        private PaletteManager _paletteManager;
        private Dictionary<ViewModelEnum, BaseViewModel> _viewModelsDictionary;

        public ManagerViewModel(Data data)
        {
            _data = data;
            _paletteManager = new PaletteManager(new Palette());
            DialogViewsManager = new DialogViewsManager();
            ToolPanel = new ToolPanelViewModel(_data, DialogViewsManager);
            _viewModelsDictionary = new Dictionary<ViewModelEnum, BaseViewModel>();
            _viewModelsDictionary.Add(ViewModelEnum.Month, new MonthViewModel(_data, DialogViewsManager));
            _viewModelsDictionary.Add(ViewModelEnum.Week, new WeekViewModel(_data, DialogViewsManager));
            _viewModelsDictionary.Add(ViewModelEnum.Year, new YearViewModel(_data, DialogViewsManager));
            _viewModelsDictionary.Add(ViewModelEnum.ToDo, new ToDoViewModel(_data, DialogViewsManager));
            _selectedViewModelEnum = ViewModelEnum.Month;

        }
        public BaseViewModel SelectedViewModel {  get { return _viewModelsDictionary[_selectedViewModelEnum]; } }
        public BaseViewModel ToolPanel  
        { 
            get => _toolPanel;
            private set 
            {
                if (_toolPanel is ToolYearPanelViewModel)
                {
                    ((ToolYearPanelViewModel)_toolPanel).SelectedEventAction -= ((YearViewModel)_viewModelsDictionary[ViewModelEnum.Year]).DoEventSelected;
                    ((ToolYearPanelViewModel)_toolPanel).EditEventAction -= ((YearViewModel)_viewModelsDictionary[ViewModelEnum.Year]).DoEditEvent;
                }                SetField(ref _toolPanel, value);
                if (_toolPanel is ToolYearPanelViewModel)
                {
                    ((ToolYearPanelViewModel)_toolPanel).SelectedEventAction += ((YearViewModel)_viewModelsDictionary[ViewModelEnum.Year]).DoEventSelected;
                    ((ToolYearPanelViewModel)_toolPanel).EditEventAction += ((YearViewModel)_viewModelsDictionary[ViewModelEnum.Year]).DoEditEvent;
                }
            } 
        }
        public IDialogViewsManager DialogViewsManager { get; }

        public ViewModelEnum SelectiewModelEnum
        {
            get { return (_selectedViewModelEnum); }
            private set 
            { 
                SetField(ref _selectedViewModelEnum, value);
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        #region Command
        public ICommand MonthViewCommand { get => new ActionCommand(ViewMonth); }
        public ICommand WeekViewCommand { get => new ActionCommand(ViewWeek); }
        public ICommand YearViewCommand { get => new ActionCommand(ViewYear); }
        public ICommand ToDoViewCommand { get => new ActionCommand(ViewToDo); }
        public ICommand SettingsCommand { get => new ActionCommand(DoSettingsCommand); }
        private void DoSettingsCommand(object o)
        {
            DialogViewsManager.ShowDialogView(new SettingsViewModel(DialogViewsManager.ClosePopUpViewCommand, _paletteManager), true);
        }
        private void ViewMonth(object o)
        {
            ToolPanel = new ToolPanelViewModel(_data, DialogViewsManager);
            SelectiewModelEnum = ViewModelEnum.Month;
        }
        private void ViewWeek(object o)
        {
            ToolPanel = new ToolPanelViewModel(_data, DialogViewsManager);
            SelectiewModelEnum = ViewModelEnum.Week;
        }
        private void ViewYear(object o)
        {
            SelectiewModelEnum = ViewModelEnum.Year;
            ToolPanel = new ToolYearPanelViewModel(_data, DialogViewsManager);
        }
        private void ViewToDo(object o)
        {
            ToolPanel = null;
            SelectiewModelEnum = ViewModelEnum.ToDo;
        }
        #endregion
    }
}
