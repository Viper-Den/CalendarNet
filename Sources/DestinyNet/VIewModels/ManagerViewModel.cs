using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Input;
using Destiny.Core;
using System;

namespace DestinyNet.ViewModels
{
    public class ManagerViewModel :  BaseViewModel
    {
        private readonly Data _data;
        private ViewModelEnum _selectedViewModelEnum;
        private BaseViewModel _toolPanel;
        private PaletteManager _paletteManager;
        private WeatherViewModel _weatherViewModel;
        private Settings _settings;
        private Dictionary<ViewModelEnum, BaseViewModel> _viewModelsDictionary;

        public ManagerViewModel(Data data, Settings settings, WeatherViewModel weatherViewModel)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _weatherViewModel = weatherViewModel ?? throw new ArgumentNullException(nameof(weatherViewModel));
            _paletteManager = new PaletteManager(_settings.Palettes);
            DialogViewsManager = new DialogViewsManager();
            ToolPanel = new ToolPanelViewModel(_data, DialogViewsManager);
            _viewModelsDictionary = new Dictionary<ViewModelEnum, BaseViewModel>();
            _viewModelsDictionary.Add(ViewModelEnum.Month, new MonthViewModel(_data, DialogViewsManager, _weatherViewModel));
            _viewModelsDictionary.Add(ViewModelEnum.Week, new WeekViewModel(_data, DialogViewsManager, _weatherViewModel, settings.WeekSettings));
            _viewModelsDictionary.Add(ViewModelEnum.Year, new YearViewModel(_data, DialogViewsManager));
            _viewModelsDictionary.Add(ViewModelEnum.People, new PeopleViewModel(_data, DialogViewsManager));
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
        public ICommand MonthViewCommand { get => new ActionCommand(DoViewMonthCommand); }
        public ICommand WeekViewCommand { get => new ActionCommand(DoViewWeekCommand); }
        public ICommand YearViewCommand { get => new ActionCommand(DoViewYearCommand); }
        public ICommand ToDoViewCommand { get => new ActionCommand(DoViewToDoCommand); }
        public ICommand PeopleViewCommand { get => new ActionCommand(DoPeopleViewCommand); }

        private void DoPeopleViewCommand(object obj)
        {
        }

        public ICommand SettingsCommand { get => new ActionCommand(DoSettingsCommand); }
        
        private void DoSettingsCommand(object o)
        {
            DialogViewsManager.ShowDialogView(new SettingsViewModel(DialogViewsManager.ClosePopUpViewCommand, _paletteManager, _settings), true);
        }
        private void DoViewMonthCommand(object o)
        {
            ToolPanel = new ToolPanelViewModel(_data, DialogViewsManager);
            SelectiewModelEnum = ViewModelEnum.Month;
        }
        private void DoViewWeekCommand(object o)
        {
            ToolPanel = new ToolPanelViewModel(_data, DialogViewsManager);
            SelectiewModelEnum = ViewModelEnum.Week;
        }
        private void DoViewYearCommand(object o)
        {
            SelectiewModelEnum = ViewModelEnum.Year;
            ToolPanel = new ToolYearPanelViewModel(_data, DialogViewsManager);
        }
        private void DoViewToDoCommand(object o)
        {
            ToolPanel = null;
            SelectiewModelEnum = ViewModelEnum.ToDo;
        }
        #endregion
    }
}
