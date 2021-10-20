using MonthEvent;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Destiny.Core;
using System.Collections.Generic;

namespace DestinyNet.ViewModels
{
    public class MonthViewModel : ViewModeDataBase
    {
        private WeatherViewModel _weatherViewModel;
        private Palette _palette;
        public MonthViewModel(Data data, IDialogViewsManager dialogViewsManager, WeatherViewModel weatherViewModel) : base(data, dialogViewsManager)
        {
            _weatherViewModel = weatherViewModel ?? throw new ArgumentNullException(nameof(weatherViewModel));
            Palette = new PaletteMounthEvent();
        }
        private void OnAddEvent(object o)
        {
            if (o is DateTime)
            {
                _dialogViewsManager.ShowDialogView(EventEditorViewModel.EventEditorViewModelNewAllDay(_dialogViewsManager.ClosePopUpViewCommand, _data, ((DateTime)o)), true);
            }
        }
        private void DoSelectedEvent(object o)
        {
            if (o is Event)
            {
                _dialogViewsManager.ShowDialogView(EventEditorViewModel.EventEditorViewModelEdit(_dialogViewsManager.ClosePopUpViewCommand, _data, (Event)o), true);
            }
        }
        public Palette Palette
        { 
            get => _palette; 
            private set => SetField(ref _palette, value); 
        }
        public Dictionary<DateTime, IDayWeather> DayWeatherCollection { get => _weatherViewModel.DayWeatherCollection; }
        public ObservableCollection<Event> Events {get => _data.Events; }
        public ICommand AddEventCommand { get => new ActionCommand(OnAddEvent); }
        public ICommand SelectedEvent { get => new ActionCommand(DoSelectedEvent); }
    }
}
