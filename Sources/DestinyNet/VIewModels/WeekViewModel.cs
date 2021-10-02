using MonthEvent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Destiny.Core;
using System.Windows.Input;

namespace DestinyNet
{
    public class WeekViewModel : ViewModeDataBase
    {
        private Palette _palette;
        private WeekSettings _weekSettings;

        private WeatherViewModel _weatherViewModel;
        public WeekViewModel(Data data, IDialogViewsManager dialogViewsManager, WeatherViewModel weatherViewModel, WeekSettings weekSettings) : base(data, dialogViewsManager)
        {
            _weekSettings = weekSettings ?? throw new ArgumentNullException(nameof(weekSettings));
            _weatherViewModel = weatherViewModel ?? throw new ArgumentNullException(nameof(weatherViewModel));
            IgnoreHours = new ObservableCollection<int>();
            Palette = new PaletteWeekEvent();
            for (var i = 0; i < 9; i++)  // скроем все часы до 9 утра
                IgnoreHours.Add(i);
            HourHeight = 50;
        }
        private void OnAddEvent(object o)
        {
            if (o is DateTime)
            {
                _dialogViewsManager.ShowDialogView(EventEditorViewModel.EventEditorViewModelEditWeek(_dialogViewsManager.ClosePopUpViewCommand, _data, ((DateTime)o)), true);
            }
        }
        private void DoSelectedEvent(object o)
        {
            if (o is Event)
            {
                _dialogViewsManager.ShowDialogView(EventEditorViewModel.EventEditorViewModelEdit(_dialogViewsManager.ClosePopUpViewCommand, _data, (Event)o), true);
            }
        }
        public int HourHeight
        {
            get => _weekSettings.HourHeight;
            set
            {
                _weekSettings.HourHeight = value;
                OnPropertyChanged(HourHeight);
            }
        }
        public bool HoursHided
        {
            get => _weekSettings.HoursHided;
            set { 
                _weekSettings.HoursHided = value;
                OnPropertyChanged(HoursHided);
            }
        }
        public bool IsEventsViewHide
        {
            get => _weekSettings.IsEventsViewHide;
            set
            {
                _weekSettings.IsEventsViewHide = value;
                OnPropertyChanged(IsEventsViewHide); 
            }
        }
        
        public Dictionary<DateTime, IDayWeather> DayWeatherCollection { get => _weatherViewModel.DayWeatherCollection; }
        public ObservableCollection<int> IgnoreHours { get; set; }
        public ObservableCollection<Event> Events { get => _data.Events; }
        public Palette Palette { get => _palette; private set => SetField(ref _palette, value); }
        public ICommand AddEventCommand { get => new ActionCommand(OnAddEvent); }
        public ICommand SelectedEvent { get => new ActionCommand(DoSelectedEvent); }
    }
}

