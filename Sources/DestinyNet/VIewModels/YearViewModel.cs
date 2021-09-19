using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Input;
using Destiny.Core;

namespace DestinyNet
{
    public class YearViewModel : ViewModeDataBase
    {
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
                    if(SelectedDates.Contains(d))
                        SelectedDates.Remove(d);
                    else
                        SelectedDates.Add(d);
                }
            }
        }
        public Boolean IsMultipleSelection { get; set; }
        public ObservableCollection<Event> Event { get; set; }
        public ObservableCollection<DateTime> SelectedDates { get; protected set; }
        public ICommand StartDateCommand { get => new ActionCommand(OnStartDate); }
        public ICommand FinishDateCommand { get => new ActionCommand(OnFinishDate); }
        public ICommand PeriodSelectedCommand { get => new ActionCommand(OnPeriodSelectedCommand); }

    }
}
