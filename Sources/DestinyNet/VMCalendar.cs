using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using UIMonthControl;

namespace DestinyNet
{

    public class DateRange : IDateRange
    {
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
    }

    public class VMCalendar : INotifyPropertyChanged
    {
        private ObservableCollection<IDateRange> _DateRanges;

        public VMCalendar()
        {
            _DateRanges = new ObservableCollection<IDateRange>();
            _DateRanges.Add(new DateRange() { Start = DateTime.Parse("2021-11-08"), Finish = DateTime.Parse("2021-11-11") });
            _DateRanges.Add(new DateRange() { Start = DateTime.Parse("2021-10-01"), Finish = DateTime.Parse("2021-10-11") });
        }

        public ObservableCollection<IDateRange> DateRanges
        {
            get{ return _DateRanges; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
