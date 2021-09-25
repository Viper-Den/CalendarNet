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
        public YearViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data, dialogViewsManager)
        {
            SelectedDates = new ObservableCollection<DateTime>();
        }
        public void OnStartDate(object o)
        {
        }
        public void OnFinishDate(object o)
        {
        }
        public void OnPeriodSelectedCommand(object o)
        {
            if (o is List<DateTime>)
            {
                var l = (List<DateTime>)o;
                if (!IsMultipleSelection)
                    SelectedDates.Clear();
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
                TitleTip = $"Выходные({hd}) Рабочие({SelectedDates.Count - hd})";
        }

        public string TitleTip 
        { 
            get => _titleTip; 
            set { SetField(ref _titleTip, value); } 
        }

        public void DoEventSelected(Event ev)
        {
            SelectedDates.Clear();
            if (ev == null)
                return;
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
        public Boolean IsMultipleSelection { get; set; }
        public ObservableCollection<Event> Event { get; set; }
        public ObservableCollection<DateTime> SelectedDates { get; protected set; }
        public ICommand StartDateCommand { get => new ActionCommand(OnStartDate); }
        public ICommand FinishDateCommand { get => new ActionCommand(OnFinishDate); }
        public ICommand PeriodSelectedCommand { get => new ActionCommand(OnPeriodSelectedCommand); }
    }
}
