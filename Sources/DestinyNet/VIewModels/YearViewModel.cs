using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Input;
using Destiny.Core;

namespace DestinyNet
{
    public class YearViewModel : ViewModeDataBase
    {
        private string _titleTip;
        private Palette _palette;
        public YearViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data, dialogViewsManager)
        {
            SelectedDates = new ObservableCollection<DateTime>();
            Palette = new PaletteYear();
        }
        private void DoAddEvent(Object o)
        {
            if (SelectedDates.Count == 0)
                return;
            _dialogViewsManager.ShowDialogView(EventEditorViewModel.EventEditorViewModelAddCollectionDays(_dialogViewsManager.ClosePopUpViewCommand, _data, SelectedDates), true);
        }
        private void DoEditeEvent(Object o)
        {
            if (o is Event)
            {
                _dialogViewsManager.ShowDialogView(EventEditorViewModel.EventEditorViewModelEdit(_dialogViewsManager.ClosePopUpViewCommand, _data, (Event)o), true);
            }
        }
        private void OnStartDate(object obj)
        {
            if (!IsMultipleSelection)
                SelectedDates.Clear();
            Palette.Selected.SetEvementStyle(Palette.SelectedDefault);
        }

        public void OnPeriodSelectedCommand(object o)
        {
            if (o is List<DateTime>)
            {
                var l = (List<DateTime>)o;
                if (!IsMultipleSelection)
                {
                    SelectedDates.Clear();
                    Palette.Selected.SetEvementStyle(Palette.SelectedDefault);
                    Palette.Selected.Background = Palette.SelectedDefault.Background;
                    Palette.Selected.Foreground = Palette.SelectedDefault.Foreground;
                }
                foreach (var d in l)
                {
                    if (SelectedDates.Contains(d))
                        SelectedDates.Remove(d);
                    else
                        SelectedDates.Add(d);
                }
                ViewSelectedDates();
            }
        }
        private void ViewSelectedDates()
        {
            var hd = 0;
            foreach (var d in SelectedDates)
            {
                if ((d.DayOfWeek == DayOfWeek.Saturday) || (d.DayOfWeek == DayOfWeek.Sunday))
                    hd++;
            }
            if (SelectedDates.Count == 0)
                TitleTip = "";
            else
                TitleTip = $"Всего[{SelectedDates.Count}] Выходные[{hd}] Рабочие[{SelectedDates.Count - hd}]";
        }

        public string TitleTip 
        { 
            get => _titleTip; 
            set { SetField(ref _titleTip, value); } 
        }
        public void DoEditEvent(Event ev)
        {
            _dialogViewsManager.ShowDialogView(EventEditorViewModel.EventEditorViewModelEditeCollectionDays(_dialogViewsManager.ClosePopUpViewCommand, _data, SelectedDates, ev));
        }
        public void DoEventSelected(Event ev)
        {
            SelectedDates.Clear();

            if (ev == null)
                return;
            Palette.Selected.Background = ev.Calendar.Background;
            Palette.Selected.Foreground = ev.Calendar.Foreground;
            var sd = new DateTime(Date.Year, 1, 1);
            var fd = new DateTime(Date.AddYears(1).Year, 1, 1);
            while(sd.Date < fd.Date)
            {
                if (ev.Rule.IsDate(sd))
                    SelectedDates.Add(sd);
                
                sd = sd.AddDays(1);
            }
            ViewSelectedDates();
        }
        public Palette Palette { 
            get => _palette; 
            private set => SetField(ref _palette, value); 
        }
        public bool IsMultipleSelection { get; set; }
        public ObservableCollection<Event> Event { get; set; }
        public ObservableCollection<DateTime> SelectedDates { get; protected set; }
        public ICommand AddEventCommand { get => new ActionCommand(DoAddEvent); }
        public ICommand EditeEventCommand { get => new ActionCommand(DoEditeEvent); }
        public ICommand StartDateCommand { get => new ActionCommand(OnStartDate); }
        //public ICommand FinishDateCommand { get => new ActionCommand(OnFinishDate); }
        public ICommand PeriodSelectedCommand { get => new ActionCommand(OnPeriodSelectedCommand); }
    }
}
