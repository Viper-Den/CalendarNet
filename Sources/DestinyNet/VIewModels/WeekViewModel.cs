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
        private int _hourHeight;
        public WeekViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data, dialogViewsManager)
        {
            IgnoreHours = new ObservableCollection<int>();
            //for (var i = 0; i < 9; i++)
            //    IgnoreHours.Add(i);
            HourHeight = 40;
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
            get => _hourHeight;
            set { SetField(ref _hourHeight, value); } 
        }
        public ObservableCollection<int> IgnoreHours { get; set; }
        public ObservableCollection<Event> Events { get => _data.Events; }
        public ICommand AddEventCommand { get => new ActionCommand(OnAddEvent); }
        public ICommand SelectedEvent { get => new ActionCommand(DoSelectedEvent); }
    }
}

