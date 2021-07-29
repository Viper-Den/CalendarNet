using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using UIMonthControl;

namespace DestinyNet
{
    public class YearViewModel : ViewModeDataBase
    {
        private DateTime _startDate;
        public YearViewModel(Data data) : base(data)
        {
            DateRanges = new ObservableCollection<IDateRange>();
            Update();
            StartDateCommand = new ActionCommand(OnStartDate);
            FinishDateCommand = new ActionCommand(OnFinishDate);
        }
        public void OnStartDate(object o)
        {
            if(o is DateTime)
            {
                _startDate = (DateTime)o;
            }
        }
        public void OnFinishDate(object o)
        {
            if (o is DateTime)
            {
                DateRanges.Add(new DateRange() {Finish = (DateTime)o, Start = _startDate, Calendar = _data.Calendars[0]});
            }
        }
        public void Update()
        {
            DateRanges.Clear();
            foreach (var r in _data.DateRanges)
            {
                DateRanges.Add(r);
            }
            OnPropertyChanged(nameof(DateRanges));
        }
        public ObservableCollection<IDateRange> DateRanges { get; }
        public ICommand StartDateCommand { get; }
        public ICommand FinishDateCommand { get; }

    }
}
