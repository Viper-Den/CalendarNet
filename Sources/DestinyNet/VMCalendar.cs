using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;
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
        private ObservableCollection<ICalendar> _Calendas; 
        private DateTime _dateYear;
        private DateTime _dateMonth;
        
        public VMCalendar()
        {
            _dateYear = DateTime.Now;
            _dateMonth = DateTime.Now;

            _DateRanges = new ObservableCollection<IDateRange>();
            _DateRanges.Add(new DateRange() { Start = DateTime.Parse("2021-11-08"), Finish = DateTime.Parse("2021-11-11") });
            _DateRanges.Add(new DateRange() { Start = DateTime.Parse("2021-10-01"), Finish = DateTime.Parse("2021-10-11") });

            _Calendas = new ObservableCollection<ICalendar>();
            _Calendas.Add(new Calendar() { Name = "Work", Enabled = true, Color = Brushes.Blue });
            _Calendas.Add(new Calendar() { Name = "Home", Enabled = true, Color = Brushes.Green });
        }

        public ObservableCollection<IDateRange> DateRanges
        {
            get { return _DateRanges; }
            set
            {
                _DateRanges = value;
                OnPropertyChanged("DateRanges");
            }
        }
        public ObservableCollection<ICalendar> Calendars
        {
            get { return _Calendas; }
            set
            {
                _Calendas = value;
                OnPropertyChanged("Calendars");
            }
        }

        public DateTime DateYear
        {
            get { return _dateYear; }
            set
            {
                _dateYear = value;
                OnPropertyChanged("DateYear");
            }
        }
        public DateTime DateMonth
        {
            get { return _dateMonth; }
            set
            {
                _dateMonth = value;
                OnPropertyChanged("DateMonth");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
